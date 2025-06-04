using AutoMapper;
using ConsoleApp1.Domain.Entities;
using ConsoleApp1.DTOs.Request;
using ConsoleApp1.DTOs.Response;

namespace ConsoleApp1.Application.Mapping
{
    
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> Response DTO
            CreateMap<User, UserResponse>();
            CreateMap<Order, OrderResponse>();
        
     

            // Request DTO -> Entity
            CreateMap<AddToBasketRequest, BasketItem>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<UserCreateRequest, User>();
            CreateMap<UserUpdateRequest, User>();

            CreateMap<ProductCreateRequest, Product>();
            CreateMap<ProductUpdateRequest, Product>();
            CreateMap<Product, ProductResponse>();
            
            CreateMap<Basket, BasketResponse>()
                .ForMember(dest => dest.BasketId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BasketItems, opt => opt.MapFrom(src => src.Items));

            CreateMap<BasketItem, BasketItemResponse>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
            
        }
    }
    
}