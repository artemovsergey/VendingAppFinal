using VendingApp.API.Controllers;

namespace VendingApp.API.Models;

public class Maintenance : TEntity
{
    public int Id { get; set; }

    public int VendingMachineId { get; set; }
    public VendingMachine? VendingMachine { get; set; }

    public DateOnly MaintenanceDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Problems { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User? User { get; set; }
}
