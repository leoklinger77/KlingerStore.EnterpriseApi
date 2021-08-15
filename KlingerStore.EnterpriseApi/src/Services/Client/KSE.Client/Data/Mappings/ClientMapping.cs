using KSE.Core.DomainObjets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KSE.Client.Data.Mappings
{
    public class ClientMapping : IEntityTypeConfiguration<Models.Client>
    {
        public void Configure(EntityTypeBuilder<Models.Client> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.OwnsOne(x => x.Cpf, tf =>
              {
                  tf.Property(c => c.Numero)
                  .IsRequired()
                  .HasMaxLength(Cpf.CpfMaxLength)
                  .HasColumnName("Cpf")
                  .HasColumnType($"varchar({Cpf.CpfMaxLength})");
              });

            builder.OwnsOne(x => x.Email, tf =>
            {
                tf.Property(c => c.EmailAddress)
                .IsRequired()
                .HasMaxLength(Email.MaxLength)
                .HasColumnName("Email")
                .HasColumnType($"varchar({Email.MaxLength})");
            });
            builder.HasOne(x => x.Address).WithOne(x => x.Client);

            builder.ToTable("TB_Client");
        }
    }
}
