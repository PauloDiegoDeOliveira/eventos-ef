using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Infrastructure.Configurations.Base;

namespace ObjetivoEventos.Infrastructure.Configurations
{
    public class SetorConfiguration : ConfigurationBase<Setor>
    {
        public override void Configure(EntityTypeBuilder<Setor> builder)
        {
            tableName = "Setores";

            base.Configure(builder);

            builder.Property(p => p.Nome)
                    .IsRequired()
                    .HasColumnName("Nome")
                    .HasMaxLength(300)
                    .HasColumnType("varchar(300)");

            builder.Property(p => p.Preco)
                   .IsRequired()
                   .HasMaxLength(10000)
                   .HasColumnName("Preco")
                   .HasColumnType("real");

            builder.Property(p => p.Posicao)
                    .IsRequired()
                    .HasColumnName("Posicao")
                    .HasMaxLength(300)
                    .HasColumnType("varchar(300)");

            builder.Property(p => p.Alias)
                    .IsRequired()
                    .HasColumnName("Alias")
                    .HasMaxLength(300)
                    .HasColumnType("varchar(300)");
        }
    }
}