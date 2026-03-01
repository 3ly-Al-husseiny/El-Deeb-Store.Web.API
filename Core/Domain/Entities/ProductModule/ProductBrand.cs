using Domain.Entities.Shared;

namespace Domain.Entities.ProductModule;

public class ProductBrand : BaseEntity<int>
{
    public string Name { get; set; } = String.Empty; 
}