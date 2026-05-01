using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendingApp.API.Data;
using VendingApp.API.Models;

namespace VendingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(VendingAppContext db) : ControllerBase
{
    /// <summary>
    /// Продажи за 10 дней c фильтрацией по сумме и количеству
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Продажи за 10 дней c фильтрацией по сумме и количеству")]
    public ActionResult<List<Sale>> GetSales(decimal amount, int count)
    {
        var salesResult = db.Sales.Where(s => s.SaleDate >= DateTime.UtcNow.AddDays(-10));
        salesResult = salesResult.Where(s => s.Amount == amount && s.Count == count);

        return Ok(salesResult.ToList());
    }
}
