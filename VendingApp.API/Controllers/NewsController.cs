using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendingApp.API.Response;

namespace VendingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    /// <summary>
    /// Новости франчайзера
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Новости франчайзера")]
    public ActionResult GetNews()
    {
        return Ok();
    }
}
