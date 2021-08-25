using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSE.Payment.Data.Mappings
{
    public class PaymentTransactionMapping : IEntityTypeConfiguration<Models.Payment>
    {
        public void Configure(EntityTypeBuilder<Models.Payment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Ignore(c => c.CreditCart);
            
            builder.HasMany(c => c.Transaction)
                .WithOne(c => c.Payment)
                .HasForeignKey(c => c.PaymentId);

            builder.ToTable("TB_Payment");
        }
    }
}
