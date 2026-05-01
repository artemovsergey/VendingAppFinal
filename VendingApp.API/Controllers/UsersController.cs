using Magicodes.ExporterAndImporter.Csv;
using Magicodes.ExporterAndImporter.Html;
using Magicodes.ExporterAndImporter.Pdf;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendingApp.API.Data;
using VendingApp.API.Dto;
using VendingApp.API.Mappers;
using VendingApp.API.Models;
using VendingApp.API.Response;

namespace VendingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(VendingAppContext db) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Получение списка пользователей")]
    public ActionResult<List<User>> GetUsers()
    {
        return Ok(db.Users.Select(u => u.ToDto()));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Создание пользователя")]
    public ActionResult<Result<User>> CreateUser(RegisterDto registerDto)
    {
        var user = new User()
        {
            Login = registerDto.Login,
            HashPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, workFactor: 12),
        };

        db.Users.Add(user);
        db.SaveChanges();

        return Created("", Result<User>.Success(user));
    }

    [SwaggerOperation(Summary = "Поиск пользователя по id")]
    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(int id)
    {
        var user = db.Users.Where(u => u.Id == id).FirstOrDefault();

        return user != null ? Ok() : throw new Exception("Сообщение от конструктоора Exception");
    }

    [HttpGet("export/csv")]
    [SwaggerOperation(Summary = "Экспорт в csv")]
    public async Task<FileContentResult> ExportToCsv()
    {
        var data = db.Set<User>().ToList();
        var csvExporter = new CsvExporter();
        var csvBytes = await csvExporter.ExportAsByteArray(data); // Возвращает byte[]
        return File(csvBytes, $"text/csv", $"export_{nameof(User)}.csv");
    }

    [HttpGet("export/html")]
    [SwaggerOperation(Summary = "Экспорт в html")]
    public async Task<ContentResult> ExportToHtml()
    {
        var data = db.Set<User>().ToList();
        var htmlExporter = new HtmlExporter();
        var htmlString = await htmlExporter.ExportListByTemplate(data); // Возвращает string (HTML-код)
        return Content(htmlString, "text/html");
    }

    [HttpGet("export/pdf")]
    [SwaggerOperation(Summary = "Экспорт в pdf")]
    public async Task<FileContentResult> ExportToPdf()
    {
        var data = db.Set<User>().ToList();
        var pdfExporter = new PdfExporter();
        var pdfBytes = await pdfExporter.ExportListBytesByTemplate(data, ""); // Возвращает byte[]
        return File(pdfBytes, "application/pdf", "export.pdf");
    }
}
