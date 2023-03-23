using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace RestaurantAPI.Services
{

    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int CreateRestaurant(CreateRestaurantDto restaurant);
        public Restaurant Delete(int id);
        public Restaurant Update(UpdateClass update);


    }



    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;

        public RestaurantService(IMapper mapper, RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var result = _dbContext
                        .Restaurants
                        .Include(r => r.Adress)
                        .Include(r => r.Dishes)
                        .ToList();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(result);

            return restaurantsDtos;
        }

        public RestaurantDto GetById(int id)
        {
            var result = _dbContext.Restaurants
                        .Include(r => r.Adress)
                        .Include(r => r.Dishes)
                        .FirstOrDefault(x => x.Id == id);

            if (result == null) return null;

            var restaurantDto = _mapper.Map<RestaurantDto>(result);

            return restaurantDto;

        }

        public int CreateRestaurant(CreateRestaurantDto restaurant)
        {
            var result = _mapper.Map<Restaurant>(restaurant);
            _dbContext.Restaurants.Add(result);
            _dbContext.SaveChanges();

            return result.Id;
        }

        public Restaurant Delete(int id)
        {
            var result = Find(id);

            if (result == null) return null;

            _dbContext.Restaurants.Remove(result);
            _dbContext.SaveChanges();

            return result;
        }

        public Restaurant Update(UpdateClass update)
        {

            var result = Find(update.id);

            if (result == null) return null;

            if (update.Name != null) result.Name = update.Name;
            if (update.Description != null) result.Description = update.Description;
            if (update.HasDelivery != null) result.HasDelivery = (bool)update.HasDelivery;

            _dbContext.SaveChanges();

            return result;


        }

        private Restaurant Find(int id)
        {
            var result = _dbContext.Restaurants
                        .FirstOrDefault(x => x.Id == id);
            return result;
        }
    }
}
