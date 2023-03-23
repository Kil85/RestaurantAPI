using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using System.Net;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seeder()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = AddRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
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
