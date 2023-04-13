namespace RestaurantAPI.Models
{
    public enum OrderBy
    {
        ASC,
        DESC
    }

    public class RestaurantQuery
    {
        public string UsersDescription { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public OrderBy OrderBy { get; set; }
        public string SortBy { get; set; }
    }
}
