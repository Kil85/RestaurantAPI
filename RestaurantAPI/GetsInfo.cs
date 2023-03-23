namespace RestaurantAPI
{
    public class GetsInfo
    {


        public int Amount { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        public GetsInfo(int amount, int min, int max)
        {
            Amount = amount;
            Min = min;
            Max = max;
        }

    }
}
