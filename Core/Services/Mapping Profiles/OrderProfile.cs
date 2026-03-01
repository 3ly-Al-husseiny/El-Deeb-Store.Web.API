using AutoMapper;
using Domain.Entities.OrderModule;
using Shared.DTOs.OrderModule;
using ShippingAddress = Domain.Entities.OrderModule.Address;
using IdentityAddress = Domain.Entities.Identity.Address;

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
        CreateMap<ShippingAddress,AddressDto>().ReverseMap();
        CreateMap<IdentityAddress,AddressDto>().ReverseMap();
    }
}