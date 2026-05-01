using Microsoft.EntityFrameworkCore;
using VendingApp.API.Models;

namespace VendingApp.API.Data;

public class VendingAppContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<VendingMachine> VendingMachines { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Maintenance> Maintenances { get; set; }

    static VendingAppContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=VendingAppDatabase;Username=postgres;Password=root"
        );
    }
}
