namespace VendingApp.API.Models;

public class VendingMachine
{
    public int Id { get; set; }
    public string Localisation { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime InstallDate { get; set; } = DateTime.Now;
    public DateTime LastServiceDate { get; set; } = DateTime.Now;
    public decimal TotalIncome { get; set; }
}
