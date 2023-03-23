using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI
{
    public class RestaurantMappPofile : Profile

    {
        public RestaurantMappPofile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(x => x.City, c => c.MapFrom(s => s.Adress.City))
                .ForMember(x => x.Steet, c => c.MapFrom(s => s.Adress.Street))
                .ForMember(x => x.PostalCode, c => c.MapFrom(s => s.Adress.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(x => x.Adress, c => c.MapFrom(s => new Adress() { City = s.City, Street = s.City, PostalCode = s.PostalCode }));
        }
    }
}
