using VendingApp.API.Dto;
using VendingApp.API.Models;

namespace VendingApp.API.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Login = user.Login,
            Role = user.Role,
        };
    }
}
