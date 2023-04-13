namespace RestaurantAPI.Models
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemTo { get; set; }
        public int TotalItemCount { get; set; }

        public PageResult(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalItemCount = totalCount;
            ItemFrom = pageSize * (pageNumber - 1) + 1;
            ItemTo = pageSize * (pageNumber);
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
