using Microsoft.AspNetCore.Identity;

namespace RestaurantAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HashedPassword { get; set; }
        public string Mail { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
