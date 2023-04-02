using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int CreateRestaurant(CreateRestaurantDto restaurant, int createdById);
        RestaurantDto Delete(int id, int userId);
        RestaurantDto Update(UpdateClass update, int userId);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(
            IMapper mapper,
            RestaurantDbContext dbContext,
            ILogger<RestaurantService> logger
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var result = _dbContext.Restaurants
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

            if (result == null)
                throw new NotFoundException("Restaurant not found");

            var restaurantDto = _mapper.Map<RestaurantDto>(result);

            return restaurantDto;
        }

        public int CreateRestaurant(CreateRestaurantDto restaurant, int createdById)
        {
            var result = _mapper.Map<Restaurant>(restaurant);
            result.CreatedById = createdById;
            _dbContext.Restaurants.Add(result);
            _dbContext.SaveChanges();

            return result.Id;
        }

        public RestaurantDto Delete(int id, int userId)
        {
            var restaurant = Find(id);

            if (restaurant.CreatedById != userId)
            {
                throw new AuthorizationException("User is not the creator of the restaurant");
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

            var result = _mapper.Map<RestaurantDto>(restaurant);

            return result;
        }

        public RestaurantDto Update(UpdateClass update, int userId)
        {
            var restaurant = Find(update.id);

            if (restaurant.CreatedById != userId)
            {
                throw new AuthorizationException("User is not the creator of the restaurant");
            }

            if (update.Name != null)
                restaurant.Name = update.Name;
            if (update.Description != null)
                restaurant.Description = update.Description;
            if (update.HasDelivery != null)
                restaurant.HasDelivery = (bool)update.HasDelivery;

            _dbContext.SaveChanges();

            var result = _mapper.Map<RestaurantDto>(restaurant);


            return result;
        }

        private Restaurant Find(int id)
        {
            var result = _dbContext.Restaurants
                .Include(x => x.Adress)
                .FirstOrDefault(x => x.Id == id);

            if (result == null)
                throw new NotFoundException("Restaurant not found");

            return result;
        }
    }
}
