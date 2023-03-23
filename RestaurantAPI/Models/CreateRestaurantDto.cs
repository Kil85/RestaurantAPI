using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class CreateRestaurantDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public bool HasDelivery { get; set; }
        public string ContactMail { get; set; }
        public string ContactPhone { get; set; }

        [MaxLength(50)]
        public string City { get; set; }
        public string Steet { get; set; }
        public string PostalCode { get; set; }
    }
}
