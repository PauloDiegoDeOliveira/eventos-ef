using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Infrastructure.Configurations.Base;

namespace ObjetivoEventos.Infrastructure.Configurations
{
    public class LocalConfiguration : ConfigurationBase<Local>
    {
        public override void Configure(EntityTypeBuilder<Local> builder)
        {
            tableName = "Locais";

            base.Configure(builder);

            builder.Property(p => p.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(300)
                   .HasColumnType("varchar(300)");

            builder.Property(p => p.Telefone)
                   .HasMaxLength(50)
                   .HasColumnName("Telefone")
                   .HasColumnType("varchar(50)");

            builder.Property(p => p.CEP)
                   .IsRequired()
                   .HasColumnName("CEP")
                   .HasMaxLength(50)
                   .HasColumnType("varchar(50)");
        }
    }
}