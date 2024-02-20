using MyLittleInstagram.Core.DTOs;

namespace MyLittleInstagram.Core.Interfaces.Services;

public interface ITokenService
{
    Task<JwtAuthDto> GetTokenAsync(LoginDto request, string getIpAddress);
}