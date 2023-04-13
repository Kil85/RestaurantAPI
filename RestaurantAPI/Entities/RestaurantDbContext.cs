using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext : DbContext
    {
        private string _connectionString =
            "Server=localhost;Database=NewRestaurant;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False";
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().Property(r => r.Name).IsRequired().HasMaxLength(20);

            modelBuilder.Entity<Dish>().Property(d => d.Name).IsRequired();

            modelBuilder.Entity<Adress>().Property(a => a.Street).HasMaxLength(50);

            modelBuilder.Entity<Adress>().Property(a => a.City).HasMaxLength(50);

            modelBuilder.Entity<User>().Property(u => u.Mail).IsRequired();

            modelBuilder.Entity<Role>().Property(u => u.Name).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
