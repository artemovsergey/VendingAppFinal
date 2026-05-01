using Microsoft.AspNetCore.Mvc;
using VendingApp.API.Data;
using VendingApp.API.Dto;
using VendingApp.API.Models;
using VendingApp.API.Response;

namespace VendingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(VendingAppContext db) : ControllerBase
{
    [HttpPost]
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

    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(int id)
    {
        var user = db.Users.Where(u => u.Id == id).FirstOrDefault();

        return user != null ? Ok() : throw new Exception("Сообщение от конструктоора Exception");
    }
}
