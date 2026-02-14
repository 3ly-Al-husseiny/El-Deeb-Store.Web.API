using System.Reflection.Metadata.Ecma335;

namespace Shared.DTOs.ProductModuleDTOs;

// record --> Immutable , Compared based on value not reference.
public record ProductResultDto
{
    public int Id { get; set; } // Id is required for some process , but it will not be displayed to the user
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
}