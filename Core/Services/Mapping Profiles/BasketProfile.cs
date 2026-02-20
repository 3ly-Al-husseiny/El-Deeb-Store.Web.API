using AutoMapper;
using Domain.Entities.BasketModule;
using Shared.DTOs.BasketModule;

namespace Services.Mapping_Profiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<CustomerBasket, BasketDto>()
            .ForMember(dest => dest.BasketItemDtos, opt => opt.MapFrom(src => src.BasketItems));
        
        CreateMap<BasketDto, CustomerBasket>()
            .ForMember(dest => dest.BasketItems, opt => opt.MapFrom(src => src.BasketItemDtos));
        
        CreateMap<BasketItem, BasketItemDto>().ReverseMap();
    }
}