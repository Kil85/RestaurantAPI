using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography.X509Certificates;

namespace RestaurantAPI
{
    public class WeatherForecastService : IWeatherForecastService
    {

        private static readonly string[] Summaries = new[]
{
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        public IEnumerable<WeatherForecast> Get(GetsInfo info)
        {
            return Enumerable.Range(1, info.Amount).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(info.Min, info.Max),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
        .ToArray();
        }


        public bool Take(int Amount, MinMax minMax, GetsInfo info)
        {
            if (Amount < 1 || minMax.Max < minMax.Min) return false;

            info.Amount = Amount;
            info.Min = minMax.Min;
            info.Max = minMax.Max;

            return true;
        }
    }
}
