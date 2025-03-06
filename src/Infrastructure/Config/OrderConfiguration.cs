using System.Security.Cryptography.X509Certificates;
using Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client.Extensibility;

namespace Infrastructure.Config;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(x => x.ShippingAddress, o => o.WithOwner());
        builder.OwnsOne(x => x.PaymentSummary, o => o.WithOwner());
        builder.Property(s => s.Status).HasConversion(
            o => o.ToString(), 
            o => (OrderStatus) Enum.Parse(typeof(OrderStatus), o));
        builder.Property(x => x.Subtotal).HasColumnType("decimal(18,2)");
        builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.OrderDate).HasConversion(
            d => d.ToUniversalTime(), 
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
    }


}

