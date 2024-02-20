using MyLittleInstagram.Core.DTOs;

namespace MyLittleInstagram.Core.Interfaces.Services;

public interface IAccountService
{
    Task<UserDto> GetUserByEmailAsync(string email);
}