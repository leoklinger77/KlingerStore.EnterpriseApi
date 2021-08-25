using KSE.Payment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSE.Payment.Data.Mappings
{
    public class TaxaTransactionMapping : IEntityTypeConfiguration<TaxaTransaction>
    {
        public void Configure(EntityTypeBuilder<TaxaTransaction> builder)
        {
            builder.HasKey(c => c.Id);            

            builder.ToTable("TB_TaxaTransaction");
        }
    }
}
