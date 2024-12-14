using Microsoft.EntityFrameworkCore;
using StoreManagement.Core;
using System.IO;

namespace StoreManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        #region Dbset

        public DbSet<Users> Users{ get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Customers> Customers { get; set; }

        #endregion

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "StoreManagement.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasIndex(u => u.Username).IsUnique();
        }
    }
}
