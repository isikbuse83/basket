using AutoMapper;

using ConsoleApp1.Domain.Entities;
using ConsoleApp1.DTOs.Request;
using ConsoleApp1.DTOs.Response;
using ConsoleApp1.Reponse;

namespace ConsoleApp1.Infrastructure
{
    
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> Response DTO
            CreateMap<User, UserResponse>();
            CreateMap<Product, ProductResponse>();
            CreateMap<BasketItem, BasketItemResponse>();
            CreateMap<Basket, BasketResponse>();
            CreateMap<Order, OrderResponse>();
        
     

            // Request DTO -> Entity
            CreateMap<AddToBasketRequest, BasketItem>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<UserCreateRequest, User>();
            CreateMap<UserUpdateRequest, User>();

            CreateMap<ProductCreateRequest, Product>();
            CreateMap<ProductUpdateRequest, Product>();
            CreateMap<Product, ProductResponse>();

        }
    }
    
}