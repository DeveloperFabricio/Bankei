using Bankei.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bankei.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Investimento> Investimentos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Investimento>()
                .HasKey(i => i.Id);
        }
    }

}
