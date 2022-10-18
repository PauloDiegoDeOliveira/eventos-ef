using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ObjetivoEventos.Identity.Entitys;

namespace ObjetivoEventos.Identity.Data
{
    public class IdentityDataContext : IdentityDbContext<Usuario>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDataContext).Assembly);
            modelBuilder.HasDefaultSchema("Identity");
        }
    }
}