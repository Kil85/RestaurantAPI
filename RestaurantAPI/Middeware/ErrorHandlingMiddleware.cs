using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Middeware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public ILogger<ErrorHandlingMiddleware> _logger { get; }

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (AuthorizationException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(e.Message);
            }
            catch (UserNotFoundException userNotFound)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(userNotFound.Message);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Cos poszlo nie tak");

            }
        }
    }
}
