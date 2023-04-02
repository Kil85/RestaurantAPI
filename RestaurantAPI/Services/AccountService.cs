using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        public void Register(RegisterAccountDto dto);
        public string Login(LoginDto dto);
    }

    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(
            IMapper mapper,
            RestaurantDbContext dbContext,
            IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings
        )
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public void Register(RegisterAccountDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.HashedPassword = hashedPassword;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public string Login(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(u => u.Mail == dto.mail);

            if (user == null)
            {
                throw new UserNotFoundException("Email or Password incorect");
            }

            var passCheck = _passwordHasher.VerifyHashedPassword(
                user,
                user.HashedPassword,
                dto.password
            );

            if (passCheck == PasswordVerificationResult.Failed)
            {
                throw new UserNotFoundException("Email or Password incorect");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.ToString())
            };

            if (!user.Nationality.IsNullOrEmpty())
            {
                claims.Add(new Claim("Nationality", user.Nationality));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)
            );

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpiredDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
