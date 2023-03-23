using RestaurantAPI.Entities;

namespace RestaurantAPI.Models
{
    public class RestaurantDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }

        public string City { get; set; }
        public string Steet { get; set; }
        public string PostalCode { get; set; }

        public List<DishDto> Dishes { get; set; }
    }
}
