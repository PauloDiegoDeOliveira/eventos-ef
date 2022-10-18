using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Infrastructure.Configurations.Base;

namespace ObjetivoEventos.Infrastructure.Configurations
{
    public class EventoConfiguration : ConfigurationBase<Evento>
    {
        public override void Configure(EntityTypeBuilder<Evento> builder)
        {
            tableName = "Eventos";

            base.Configure(builder);

            builder.Property(p => p.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasMaxLength(300)
                   .HasColumnType("varchar(300)");

            builder.Property(p => p.Sobre)
                   .IsRequired()
                   .HasColumnName("Sobre")
                   .HasMaxLength(500)
                   .HasColumnType("varchar(500)");

            builder.Property(p => p.Duracao)
                   .IsRequired()
                   .HasMaxLength(10000)
                   .HasColumnName("Duracao")
                   .HasColumnType("int");

            builder.Property(p => p.Cantor)
                   .HasColumnName("Cantor")
                   .HasMaxLength(500)
                   .HasColumnType("varchar(500)");
        }
    }
}