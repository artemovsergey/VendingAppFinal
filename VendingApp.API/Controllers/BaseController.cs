using Microsoft.AspNetCore.Mvc;
using VendingApp.API.Data;

namespace VendingApp.API.Controllers;

public interface TEntity
{
    int Id { get; set; }
}

public abstract class BaseController<T>(VendingAppContext db) : ControllerBase
    where T : class, TEntity
{
    [HttpGet]
    public ActionResult<List<T>> GetAll()
    {
        return db.Set<T>().ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<T> GetById(int id)
    {
        var entity = db.Set<T>().Where(e => e.Id == id).FirstOrDefault();
        return entity != null ? Ok(entity) : NotFound();
    }
}
