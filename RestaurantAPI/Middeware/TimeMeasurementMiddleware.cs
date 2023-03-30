using System.Diagnostics;

namespace RestaurantAPI.Middeware
{
    public class TimeMeasurementMiddleware : IMiddleware
    {
        public readonly ILogger<TimeMeasurementMiddleware> _logger;
        public TimeMeasurementMiddleware(ILogger<TimeMeasurementMiddleware> logger)
        {
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch sw = Stopwatch.StartNew();
            await next.Invoke(context);
            sw.Stop();

            if (sw.ElapsedMilliseconds > 4000)
            {
                var message = $"Request: {context.Request.Method} at {context.Request.Path} took {sw.ElapsedMilliseconds}ms";
                _logger.LogInformation(message);
            }

        }
    }
}
