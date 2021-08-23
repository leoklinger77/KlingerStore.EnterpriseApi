using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSE.Payment.Data.Mappings
{
    public class TransactionMappings : IEntityTypeConfiguration<Models.Transaction>
    {
        public void Configure(EntityTypeBuilder<Models.Transaction> builder)
        {
            builder.HasKey(c => c.Id);
                        
            builder.HasOne(c => c.Payment)
                .WithMany(c => c.Transaction);

            builder.ToTable("TB_Transaction");
        }
    }
}
