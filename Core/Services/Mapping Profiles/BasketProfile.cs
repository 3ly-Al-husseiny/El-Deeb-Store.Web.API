using AutoMapper;
using Domain.Entities.BasketModule;
using Shared.DTOs.BasketModule;

namespace Services.Mapping_Profiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<CustomerBasket, BasketDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        
        CreateMap<BasketDto, CustomerBasket>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        
        CreateMap<BasketItem, BasketItemDto>().ReverseMap();
    }
}