using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        public void Register(RegisterAccountDto dto);

    }
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;


        public AccountService(IMapper mapper, RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Register(RegisterAccountDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.HashedPassword = hashedPassword;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

    }
}
