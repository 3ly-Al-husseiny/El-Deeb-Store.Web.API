using AutoMapper;
using Domain.Entities.ProductModule;
using Shared.DTOs.ProductModuleDTOs;

namespace Services.Mapping_Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductType, TypeResultDto>();
        CreateMap<ProductBrand, BrandResultDto>();
        CreateMap<Product, ProductResultDto>() // allow the loading of the related data (Brand and Type) to be mapped to the DTO
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.ProductType.Name));
    }
}