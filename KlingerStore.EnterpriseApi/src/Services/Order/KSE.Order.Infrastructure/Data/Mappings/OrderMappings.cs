using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSE.Order.Infrastructure.Data.Mappings
{
    public class OrderMappings : IEntityTypeConfiguration<Domain.Domain.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Domain.Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasDefaultValue("NEXT VALUE FOR OrderSequel");

            builder.HasMany(x => x.Itens)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            builder.ToTable("TB_Order");
        }
    }
}
