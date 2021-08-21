using KSE.Order.Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSE.Order.Infrastructure.Data.Mappings
{
    public class OrderItemMappings : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItens);

            builder.ToTable("TB_OrderItem");
        }
    }
}
