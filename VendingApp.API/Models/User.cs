namespace VendingApp.API.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;
    public RoleType Role { get; set; } = RoleType.Client;
}

public enum RoleType
{
    Admin = 0,
    Manager = 1,
    Client = 2,
}
