using AutoMapper;
using Domain.Entities.BasketModule;
using Shared.DTOs.BasketModule;

namespace Services.Mapping_Profiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<CustomerBasket, BasketDto>().ReverseMap();
        CreateMap<BasketItem, BasketItemDto>().ReverseMap();
    }
}