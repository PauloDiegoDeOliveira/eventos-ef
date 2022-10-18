using Microsoft.EntityFrameworkCore;
using ObjetivoEventos.Domain.Entitys;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ObjetivoEventos.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Local> Locais { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Cadeira> Cadeiras { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("AlteradoEm").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}