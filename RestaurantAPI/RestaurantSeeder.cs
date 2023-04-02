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

        public RestaurantSeeder(RestaurantDbContext dbContext,
            IPasswordHasher<User> password)
        {
            _dbContext = dbContext;
            _passwordHasher = password;
        }

        public void Seeder()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = AddRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = AddRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Users.Any())
                {
                    var users = AddUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> AddRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Admin",
                },
                new Role()
                {
                    Name = "User",
                },
                new Role()
                {
                    Name = "Manager",
                }

            };
            return roles;
        }

        private IEnumerable<User> AddUsers()
        {
            var admin = new User()
            {
                FirstName = "Admin",
                Mail = "admin",
                RoleId = 1
            };
            var passwordAdmin = "admin";
            var hashedPassword = _passwordHasher.HashPassword(admin, passwordAdmin);
            admin.HashedPassword = hashedPassword;

            var mod = new User()
            {
                FirstName = "Moderator",
                Mail = "moderator",
                RoleId = 3
            };
            var passwordModerator = "moderator";
            var hashedPasswordMod = _passwordHasher.HashPassword(mod, passwordModerator);
            mod.HashedPassword = hashedPasswordMod;

            var user = new User()
            {
                FirstName = "User",
                Mail = "user",
                RoleId = 2
            };
            var passwordUser = "moderator";
            var hashedPasswordUser = _passwordHasher.HashPassword(mod, passwordUser);
            user.HashedPassword = hashedPasswordMod;

            var users = new List<User>() { admin, mod, user };
            return users;

        }

        private IEnumerable<Restaurant> AddRestaurants()
        {
            var resoult = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Description =
                        "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                    ContactMail = "contact@kfc.com",
                    ContactPhone = "123456789",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Nashville Hot Chicken",
                            Description = "Chicken",
                            Price = 10,
                        },

                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Description = "Nuggets",
                            Price = 5,
                        },
                    },
                    Adress = new Adress()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-001"
                    }
                },
                new Restaurant()
                {
                    Name = "McDonald Szewska",
                    Description =
                        "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                    ContactMail = "contact@mcdonald.com",
                    ContactPhone = "987654321",
                    HasDelivery = true,
                    Adress = new Adress()
                    {
                        City = "Kraków",
                        Street = "Szewska 2",
                        PostalCode = "30-001"
                    }
                }
            };
            return resoult;
        }
    }

}
