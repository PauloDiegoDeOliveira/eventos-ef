using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Infrastructure.Configurations.Base;

namespace ObjetivoEventos.Infrastructure.Configurations
{
    public class PedidoConfiguration : ConfigurationBase<Pedido>
    {
        public override void Configure(EntityTypeBuilder<Pedido> builder)
        {
            tableName = "Pedidos";

            base.Configure(builder);

            builder.Property(p => p.SituacaoPedido)
                   .IsRequired()
                   .HasColumnName("SituacaoPedido")
                   .HasMaxLength(50)
                   .HasColumnType("varchar(50)")
                   .HasDefaultValue("AguardandoPagamento");

            builder.Property(p => p.Numero)
                   .HasColumnName("Numero")
                   .UseIdentityColumn()
                   .ValueGeneratedOnAdd()
                   .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}