using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace RestaurantAPI.Models
{
    public class RegisterAccountDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; } = 2;

    }
}
