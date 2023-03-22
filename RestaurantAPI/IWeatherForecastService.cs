
using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get(GetsInfo info);
        bool Take(int Amount, MinMax minMax, GetsInfo info);
    }
}
