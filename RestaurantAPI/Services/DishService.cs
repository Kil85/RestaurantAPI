using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{

    public interface IDishService
    {
        public int Add(int restaurantId, CreateDishDto dto);
        public DishDto GetById(int restaurantId, int dishId);
        public List<DishDto> GetAll(int restaurantId);
        public DishDto Delete(int restaurantId, int dishId);

    }


    public class DishService : IDishService

    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;

        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = context;
        }

        public int Add(int restaurantId, CreateDishDto dto)
        {
            var restaurant = FindRestaurantById(restaurantId);

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;

            _dbContext.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = FindRestaurantById(restaurantId);

            var dish = DishCheck(restaurant, dishId);

            var result = _mapper.Map<DishDto>(dish);
            return result;

        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = FindRestaurantById(restaurantId);


            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var result = _mapper.Map<List<DishDto>>(restaurant.Dishes);
            return result;
        }

        public DishDto Delete(int restaurantId, int dishId)
        {
            var restaurant = FindRestaurantById(restaurantId);

            var dish = DishCheck(restaurant, dishId);

            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();

            var result = _mapper.Map<DishDto>(dish);
            return result;

        }



        private Restaurant FindRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(d => d.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }
            return restaurant;
        }

        private Dish DishCheck(Restaurant restaurant, int dishId)
        {
            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish == null || dish.RestaurantId != restaurant.Id)
            {
                throw new NotFoundException("Dish not found");
            }

            return dish;
        }
    }
}
