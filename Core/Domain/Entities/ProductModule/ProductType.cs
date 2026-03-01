using Domain.Entities.Shared;

namespace Domain.Entities.ProductModule;

public class ProductType : BaseEntity<int>
{
    public string Name { get; set; } = String.Empty;
}