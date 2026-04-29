using Microsoft.EntityFrameworkCore;
using VendingApp.API.Models;

namespace VendingApp.API.Data;

public class VendingAppContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=VendingAppDatabase;Username=postgres;Password=root"
        );
    }
}
