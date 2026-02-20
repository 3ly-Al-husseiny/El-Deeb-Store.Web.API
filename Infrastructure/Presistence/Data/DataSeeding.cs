using System.Text.Json;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;

namespace Presistence.Data;

public class DataSeeding(
    ECommerceDbContext _dbContext,
    RoleManager<IdentityRole> _roleManager,
    UserManager<User> _userManager) : IDataSeeding
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
                var productBrandsData =
                    File.OpenRead("../Infrastructure/Presistence/Data/DataSeedingFiles/brands.json");
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

    public async Task SeedIdentityAsync()
    {
        try
        {
            // 1 Seed the roles [Admin , SuperAdmin]
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            // 2 seed users [AdminUser , SuperAdminUser]
            if (_userManager.Users.Any())
            {
                var adminUser = new User
                {
                    UserName = "Admin",
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01023211727",
                    Address = new Address
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Country = "Egypt",
                        Street = "Admin Street"
                    }
                };

                var superAdminUser = new User
                {
                    UserName = "SuperAdmin",
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01023211727",
                    Address = new Address
                    {
                        FirstName = "SuperAdmin",
                        LastName = "SuperAdmin",
                        Country = "Egypt",
                        Street = "SuperAdmin Street"
                    }
                };


                await _userManager.CreateAsync(adminUser,"AdminPa$$w0rd");
                await _userManager.CreateAsync(superAdminUser,"SuperAdminPa$$w0rd");
                // 3 Assign roles ==> users
                _userManager.AddToRoleAsync(adminUser, "Admin");
                _userManager.AddToRolesAsync(superAdminUser, new[] { "Admin", "SuperAdmin" });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}