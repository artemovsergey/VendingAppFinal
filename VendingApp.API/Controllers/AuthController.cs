using Microsoft.AspNetCore.Mvc;
using VendingApp.API.Data;
using VendingApp.API.Dto;
using VendingApp.API.Mappers;
using VendingApp.API.Models;

namespace VendingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(VendingAppContext db) : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<User> Login(LoginDto loginDto)
    {
        var user = db.Users.Where(u => u.Login == loginDto.Login).FirstOrDefault();

        if (user == null)
            return NotFound("Не найден пользователь");

        var result = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashPassword);

        if (!result)
            return Unauthorized("Неверный пароль");

        return Ok(user.ToDto());
    }
}
