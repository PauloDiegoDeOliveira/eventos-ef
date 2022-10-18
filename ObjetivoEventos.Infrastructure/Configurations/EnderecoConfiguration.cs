using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Infrastructure.Configurations.Base;

namespace ObjetivoEventos.Infrastructure.Configurations
{
    public class EnderecoConfiguration : ConfigurationBase<Endereco>
    {
        public override void Configure(EntityTypeBuilder<Endereco> builder)
        {
            tableName = "Enderecos";

            base.Configure(builder);

            builder.Property(p => p.CEP)
                  .IsRequired()
                  .HasColumnName("CEP")
                  .HasColumnType("varchar(10)");

            builder.Property(p => p.Estado)
                   .IsRequired()
                   .HasColumnName("Estado")
                   .HasColumnType("varchar(100)");

            builder.Property(p => p.Cidade)
                   .IsRequired()
                   .HasColumnName("Cidade")
                   .HasColumnType("varchar(100)");

            builder.Property(p => p.Logradouro)
                   .IsRequired()
                   .HasColumnName("Logradouro")
                   .HasColumnType("varchar(100)");

            builder.Property(p => p.Complemento)
                   .HasColumnName("Complemento")
                   .HasColumnType("varchar(100)");
        }
    }
}