using VendingApp.API.Controllers;

namespace VendingApp.API.Models;

public class Product : TEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Count { get; set; }
    public int MinCount { get; set; }
    public decimal Rating { get; set; }
}
