using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using System.Net;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RestaurantSeeder(RestaurantDbContext dbContext, IPasswordHasher<User> password)
        {
            _dbContext = dbContext;
            _passwordHasher = password;
        }

        public void Seeder(int howMany)
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = AddRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Users.Any())
                {
                    var users = AddUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = AddRestaurants(howMany);
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> AddRoles()
        {
            var roles = new List<Role>()
            {
                new Role() { Name = "Admin", },
                new Role() { Name = "User", },
                new Role() { Name = "Manager", }
            };
            return roles;
        }

        private IEnumerable<User> AddUsers()
        {
            var admin = new User()
            {
                FirstName = "Admin",
                Mail = "admin",
                RoleId = 1,
                DateOfBirth = new DateTime(2000, 2, 06)
            };
            var passwordAdmin = "admin";
            var hashedPassword = _passwordHasher.HashPassword(admin, passwordAdmin);
            admin.HashedPassword = hashedPassword;

            var mod = new User()
            {
                FirstName = "Moderator",
                Mail = "moderator",
                RoleId = 3,
                DateOfBirth = new DateTime(2000, 2, 06)
            };
            var passwordModerator = "moderator";
            var hashedPasswordMod = _passwordHasher.HashPassword(mod, passwordModerator);
            mod.HashedPassword = hashedPasswordMod;

            var user = new User()
            {
                FirstName = "User",
                Mail = "user",
                RoleId = 2,
                DateOfBirth = new DateTime(2000, 2, 06)
            };
            var passwordUser = "moderator";
            var hashedPasswordUser = _passwordHasher.HashPassword(mod, passwordUser);
            user.HashedPassword = hashedPasswordMod;

            var users = new List<User>() { admin, mod, user };
            return users;
        }

        private IEnumerable<Restaurant> AddRestaurants(int howMany)
        {
            var locale = "en";
            var randomAdress = new Faker<Adress>(locale)
                .RuleFor(a => a.City, c => c.Address.City())
                .RuleFor(a => a.Street, c => c.Address.StreetName())
                .RuleFor(a => a.PostalCode, c => c.Address.ZipCode());

            var randomDishes = new Faker<Dish>(locale)
                .RuleFor(d => d.Name, e => e.Random.String(10))
                .RuleFor(d => d.Description, e => e.Random.String(70))
                .RuleFor(d => d.Price, e => e.Random.Int(1, 1000));

            var randomRestaurant = new Faker<Restaurant>(locale)
                .RuleFor(r => r.Name, s => s.Company.CompanyName().ClampLength(1, 15))
                .RuleFor(
                    r => r.Description,
                    s => s.Random.String2(100, "abcd efgh ijkl mnop rstu wxyz")
                )
                .RuleFor(r => r.HasDelivery, s => s.Random.Bool())
                .RuleFor(r => r.ContactMail, s => s.Internet.Email())
                .RuleFor(r => r.ContactPhone, s => s.Phone.PhoneNumber("###-###-###").ToString())
                .RuleFor(r => r.CreatedById, 1)
                .RuleFor(r => r.Adress, s => randomAdress.Generate())
                .RuleFor(r => r.Dishes, s => randomDishes.Generate(6));

            var result = randomRestaurant.Generate(howMany);

            return result;
        }
    }
}
