using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Infrastructure.Configurations.Base;

namespace ObjetivoEventos.Infrastructure.Configurations
{
    public class MesaConfiguration : ConfigurationBase<Mesa>
    {
        public override void Configure(EntityTypeBuilder<Mesa> builder)
        {
            tableName = "Mesas";

            base.Configure(builder);

            builder.Property(p => p.Nome)
                    .IsRequired()
                    .HasColumnName("Nome")
                    .HasMaxLength(300)
                    .HasColumnType("varchar(300)");

            builder.Property(p => p.Alias)
                    .IsRequired()
                    .HasColumnName("Alias")
                    .HasMaxLength(300)
                    .HasColumnType("varchar(300)");

            builder.Property(p => p.Fileira)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("Fileira")
                   .HasColumnType("varchar(50)");

            builder.Property(p => p.Coluna)
                   .IsRequired()
                   .HasMaxLength(10000)
                   .HasColumnName("Coluna")
                   .HasColumnType("int");
        }
    }
}