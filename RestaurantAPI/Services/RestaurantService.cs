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
using System.Linq.Expressions;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        PageResult<RestaurantDto> GetAll(RestaurantQuery query);
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

        public PageResult<RestaurantDto> GetAll(RestaurantQuery query)
        {
            var baseRestaurants = _dbContext.Restaurants
                .Include(r => r.Adress)
                .Include(r => r.Dishes)
                .Where(
                    r =>
                        query.UsersDescription == null
                        || (
                            r.Description.ToLower().Contains(query.UsersDescription.ToLower())
                            || r.Name.ToLower().Contains(query.UsersDescription.ToLower())
                        )
                );

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var dictionary = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), r => r.Name },
                    { nameof(Restaurant.Description), r => r.Description },
                    { nameof(Restaurant.CreatedBy), r => r.CreatedBy },
                };

                var sortDecision = dictionary[query.SortBy];

                if (query.OrderBy == OrderBy.ASC)
                {
                    baseRestaurants = baseRestaurants.OrderBy(sortDecision);
                }
                else
                {
                    baseRestaurants = baseRestaurants.OrderByDescending(sortDecision);
                }
            }

            var restaurants = baseRestaurants
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new PageResult<RestaurantDto>(
                restaurantsDtos,
                baseRestaurants.Count(),
                query.PageNumber,
                query.PageSize
            );
            return result;
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
