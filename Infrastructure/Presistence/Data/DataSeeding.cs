using System.Text.Json;
using Services.Abstraction;

namespace Presistence.Data;

public class DataSeeding(ECommerceDbContext _dbContext) : IDataSeeding
{
    public void Seed()
    {
        try
        {

            //1] Apply Any Pending Migrations
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                // Apply pending migrations to the database & Create the database if it does not exist
                _dbContext.Database.Migrate();
            }
            //2] Seed Data if the tables are empty
            if (!_dbContext.ProductBrands.Any())
            {
                var productBrandsData = File.ReadAllText("../Infrastructure/Presistence/Data/DataSeedingFiles/brands.json");
                var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(productBrandsData);
                if (productBrands != null)
                {
                    _dbContext.ProductBrands.AddRange(productBrands);
                    _dbContext.SaveChanges();
                }
            }

            if (!_dbContext.ProductTypes.Any())
            {
                var productTypesData = File.ReadAllText("../Infrastructure/Presistence/Data/DataSeedingFiles/types.json");
                var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);
                if (productTypes != null)
                {
                    _dbContext.ProductTypes.AddRange(productTypes);
                    _dbContext.SaveChanges();
                }
            }

            if (!_dbContext.Products.Any())
            {
                var productsData = File.ReadAllText("../Infrastructure/Presistence/Data/DataSeedingFiles/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products != null)
                {
                    _dbContext.Products.AddRange(products);
                    _dbContext.SaveChanges();
                }
            }
        }
        catch (Exception ex)
        {
            
        }
    }
}