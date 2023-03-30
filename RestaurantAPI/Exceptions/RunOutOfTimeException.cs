namespace RestaurantAPI.Exceptions
{
    public class RunOutOfTimeException : Exception
    {
        public RunOutOfTimeException(string message) : base(message) { }
    }
}
