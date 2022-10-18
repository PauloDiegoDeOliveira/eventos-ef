using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Infrastructure.Configurations.Base;

namespace ObjetivoEventos.Infrastructure.Configurations
{
    public class ReservaConfiguration : ConfigurationBase<Reserva>
    {
        public override void Configure(EntityTypeBuilder<Reserva> builder)
        {
            tableName = "Reservas";

            base.Configure(builder);

            builder.Property(p => p.LocalId)
                   .IsRequired()
                   .HasColumnName("LocalId")
                   .HasMaxLength(300)
                   .HasColumnType("varchar(300)");

            builder.Property(p => p.MesaId)
                   .HasColumnName("MesaId")
                   .HasMaxLength(300)
                   .HasColumnType("varchar(300)");

            builder.Property(p => p.CadeiraId)
                   .IsRequired()
                   .HasColumnName("CadeiraId")
                   .HasMaxLength(300)
                   .HasColumnType("varchar(300)");

            builder.Property(p => p.SituacaoReserva)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("SituacaoReserva")
                   .HasColumnType("varchar(50)")
                   .HasDefaultValue("Reservado");
        }
    }
}