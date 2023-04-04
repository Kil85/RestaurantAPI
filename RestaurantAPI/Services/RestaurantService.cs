using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Authorization;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int CreateRestaurant(CreateRestaurantDto restaurant);
        RestaurantDto Delete(int id);
        RestaurantDto Update(UpdateClass update, int id);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(
            IMapper mapper,
            RestaurantDbContext dbContext,
            IAuthorizationService handler,
            IUserContextService userContextService
        )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authorizationService = handler;
            _userContextService = userContextService;
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

        public int CreateRestaurant(CreateRestaurantDto restaurant)
        {
            var result = _mapper.Map<Restaurant>(restaurant);
            result.CreatedById = _userContextService.GetUserId;
            _dbContext.Restaurants.Add(result);
            _dbContext.SaveChanges();

            return result.Id;
        }

        public RestaurantDto Delete(int id)
        {
            var restaurant = Find(id);

            var authorizationResult = _authorizationService
                .AuthorizeAsync(
                    _userContextService.GetUser,
                    restaurant,
                    new ResourceOperationRequirement(ResourceOperations.Delete)
                )
                .Result;

            if (!authorizationResult.Succeeded)
            {
                throw new AuthorizationException("User is not the creator of the restaurant");
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

            var result = _mapper.Map<RestaurantDto>(restaurant);

            return result;
        }

        public RestaurantDto Update(UpdateClass update, int id)
        {
            var restaurant = Find(id);

            var authorizationResult = _authorizationService
                .AuthorizeAsync(
                    _userContextService.GetUser,
                    restaurant,
                    new ResourceOperationRequirement(ResourceOperations.Update)
                )
                .Result;

            if (!authorizationResult.Succeeded)
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
