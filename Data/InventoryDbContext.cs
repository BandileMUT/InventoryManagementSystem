using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;
using Microsoft.Identity.Client;

namespace InventoryManagementSystem.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }
    }
}
