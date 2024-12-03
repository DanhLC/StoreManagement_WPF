using Microsoft.EntityFrameworkCore;
using StoreManagement.Models;

namespace StoreManagement.Data
{
    public class AppDbContext : DbContext
    {
        #region Dbset

        public DbSet<Users> Users{ get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Customer> Customers { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=StoreManagement.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasIndex(u => u.Username).IsUnique();
        }
    }
}
