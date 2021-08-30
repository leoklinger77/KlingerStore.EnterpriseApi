using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSE.Client.Data.Mappings
{
    public class PhoneMapping : IEntityTypeConfiguration<Models.Phone>
    {
        public void Configure(EntityTypeBuilder<Models.Phone> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Ddd)
                .IsRequired()
                .HasColumnType("char(2)");

            builder.Property(x => x.Number)
                .IsRequired()
                .HasColumnType("varchar(9)");

            builder.Property(x => x.PhoneType)
                .IsRequired();                

            builder.ToTable("TB_Phone");
        }
    }
}
