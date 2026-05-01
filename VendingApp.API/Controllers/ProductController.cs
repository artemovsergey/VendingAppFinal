using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendingApp.API.Data;
using VendingApp.API.Models;

namespace VendingApp.API.Controllers;

public class ProductController(VendingAppContext db) : BaseController<Product>(db)
{
    [HttpGet]
    [SwaggerOperation(Summary = "Получения списка продуктов")]
    public override ActionResult<List<Product>> GetAll()
    {
        return db.Set<Product>().ToList();
    }
}
