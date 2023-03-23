using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RestaurantAPI
{
    public class UpdateClass
    {
        [Required]
        public int id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public bool? HasDelivery { get; set; }
    }
}
