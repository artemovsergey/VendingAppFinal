namespace VendingApp.API.Models;

public class Sale
{
    public int Id { get; set; }

    public int VendingMachineId { get; set; }
    public VendingMachine? VendingMachine { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Count { get; set; }
    public decimal Amount { get; set; }
    public DateTime SaleDate { get; set; }
    public PaymentType Payment { get; set; }
}

public enum PaymentType
{
    Card = 0,
    Cash = 1,
    QR = 2,
}
