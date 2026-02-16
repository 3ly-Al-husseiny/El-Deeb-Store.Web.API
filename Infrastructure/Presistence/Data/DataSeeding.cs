using System.Text.Json;
using Services.Abstraction;

namespace Presistence.Data;

public class DataSeeding(ECommerceDbContext _dbContext) : IDataSeeding
{
    public async Task SeedAsync()
    {
        try
        {

            //1] Apply Any Pending Migrations
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                // Apply pending migrations to the database & Create the database if it does not exist
                await _dbContext.Database.MigrateAsync();
            }
            
            //2] Seed Data if the tables are empty
            if (!_dbContext.ProductBrands.Any())
            {
                var productBrandsData = File.OpenRead("../Infrastructure/Presistence/Data/DataSeedingFiles/brands.json");
                var productBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandsData);
                if (productBrands != null)
                {
                    await _dbContext.ProductBrands.AddRangeAsync(productBrands);
                }
            }

            if (!_dbContext.ProductTypes.Any())
            {
                var productTypesData = File.OpenRead("../Infrastructure/Presistence/Data/DataSeedingFiles/types.json");
                var productTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypesData);
                if (productTypes != null)
                {
                    await _dbContext.ProductTypes.AddRangeAsync(productTypes);
                }
            }

            if (!_dbContext.Products.Any())
            {
                var productsData = File.OpenRead("../Infrastructure/Presistence/Data/DataSeedingFiles/products.json");
                var products = await JsonSerializer.DeserializeAsync<List<Product>>(productsData);
                if (products != null)
                {
                    await _dbContext.Products.AddRangeAsync(products);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            
        }
    }
}