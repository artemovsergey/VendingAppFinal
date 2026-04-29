using Microsoft.AspNetCore.Mvc;
using VendingApp.API.Data;
using VendingApp.API.Dto;
using VendingApp.API.Models;

namespace VendingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(VendingAppContext db) : ControllerBase
{
    [HttpPost]
    public ActionResult<User> CreateUser(RegisterDto registerDto)
    {
        var user = new User()
        {
            Login = registerDto.Login,
            HashPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, workFactor: 12),
        };

        try
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.InnerException!.Message}");
        }

        return Created("", user);
    }
}
