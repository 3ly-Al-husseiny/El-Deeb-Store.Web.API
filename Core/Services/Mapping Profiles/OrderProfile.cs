using AutoMapper;
using Domain.Entities.OrderModule;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Shared.DTOs.OrderModule;

namespace Services.Mapping_Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<DeliveryMethod, DeliveryMethodResult>();
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.ProductId))
            .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
            .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.Product.PictureUrl));
        CreateMap<Order, OrderResult>()
            .ForMember(dest => dest.PaymentStatus, options => options.MapFrom(src => src.PaymentStatus.ToString()))
            .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod.ShortName))
            .ForMember(dest => dest.TotalPrice,
                options => options.MapFrom(src => src.SubTotal + src.DeliveryMethod.Price));
        CreateMap<AddressDto,Domain.Entities.Identity.Address>().ReverseMap();
    }
}