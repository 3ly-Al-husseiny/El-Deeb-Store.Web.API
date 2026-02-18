using Domain.Entities.Shared;

namespace Domain.Entities.ProductModule;

public class ProductBrand : BasedEntity<int>
{
    public string Name { get; set; } = String.Empty; 
}