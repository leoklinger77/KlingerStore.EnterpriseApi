using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSE.Client.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Models.Address>
    {
        public void Configure(EntityTypeBuilder<Models.Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ZipCode)
                .IsRequired()
                .HasColumnType("char(8)");

            builder.Property(x => x.Street)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Number)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(x => x.Complement)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.District)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.City)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.State)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.ToTable("TB_Address");
        }
    }
}
