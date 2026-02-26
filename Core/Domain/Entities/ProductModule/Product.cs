using Domain.Entities.Shared;

namespace Domain.Entities.ProductModule;

public class Product : BaseEntity<int>
{
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string PictureUrl { get; set; }
    public decimal Price { get; set; }
    
    // Relationships
    
    #region Many-to-One with ProductBrand
    // Foreign Key
    public int BrandId { get; set; }
    // Navigation Property
    public ProductBrand ProductBrand { get; set; }
    #endregion

    #region Many-to-One with ProductType
    // Foreign Key
    public int TypeId { get; set; }
    // Navigation Property
    public ProductType ProductType { get; set; }
    #endregion
    
    
}