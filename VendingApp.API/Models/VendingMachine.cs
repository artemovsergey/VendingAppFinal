namespace VendingApp.API.Models;

public class VendingMachine
{
    public int Id { get; set; }
    public string Localisation { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public VendingMachineType Type { get; set; } = VendingMachineType.CardAndCashPay;
    public VendingMachineStatus Status { get; set; } = VendingMachineStatus.On;
    public DateTime InstallDate { get; set; } = DateTime.Now;
    public DateTime LastServiceDate { get; set; } = DateTime.Now;
    public decimal TotalIncome { get; set; }
}

public enum VendingMachineStatus
{
    On = 0,
    Off = 1,
    Service = 2,
}

public enum VendingMachineType
{
    CardPay = 0,
    CashPay = 1,
    CardAndCashPay = 2,
}
