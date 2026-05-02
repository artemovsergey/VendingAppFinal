using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendingApp.API.Data;

namespace VendingApp.API.Controllers;

public interface TEntity
{
    int Id { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T>(VendingAppContext db) : ControllerBase
    where T : class, TEntity
{
    [HttpGet]
    [SwaggerOperation(Summary = $"Получение списка")]
    public virtual ActionResult<List<T>> GetAll()
    {
        return db.Set<T>().ToList();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = $"Поиск по id ")]
    public ActionResult<T> GetById(int id)
    {
        var entity = db.Set<T>().Where(e => e.Id == id).FirstOrDefault();
        return entity != null ? Ok(entity) : NotFound();
    }

    [HttpGet("export/pdf")]
    [SwaggerOperation(Summary = "Экспорт в pdf")]
    public ActionResult<T> ExportToPdf()
    {
        return Ok();
    }

    [HttpGet("export/html")]
    [SwaggerOperation(Summary = "Экспорт в html")]
    public ActionResult<T> ExportToHtml()
    {
        return Ok();
    }
}
