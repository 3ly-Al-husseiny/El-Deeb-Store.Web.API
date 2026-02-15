using AutoMapper;
using Domain.Entities.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.ProductModuleDTOs;

namespace Services.Mapping_Profiles;

internal class PictureURLResoulver(IConfiguration _cnfg) : IValueResolver<Product,ProductResultDto,string>
{
    public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            // get the base url from the appsettings using the configuration
            
            
            return $"{_cnfg.GetSection("URLS")["BaseURL"]}{source.PictureUrl}";
        }
        return string.Empty;
    }
}