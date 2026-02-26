using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence.Identity;

public class IdentityECommerceDbContext : IdentityDbContext
{
    public IdentityECommerceDbContext(DbContextOptions<IdentityECommerceDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Address>().ToTable("Addressess");
    }
}