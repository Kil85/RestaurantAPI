namespace RestaurantAPI.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }
        public string ContactMail { get; set; }

        public string ContactPhone { get; set; }

        public virtual Adress Adress { get; set; }
        public int AdressId { get; set; }

        public virtual List<Dish> Dishes { get; set; }

    }
}
