using VendingApp.API.Models;

namespace VendingApp.API.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public RoleType Role { get; set; }
}
