using Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations.OrderModuleConfigurations;

public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(d => d.Price).HasColumnType("decimal(18,4)");
        builder.OwnsOne(o => o.Product, p => p.WithOwner());
    }
}