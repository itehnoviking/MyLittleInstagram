using MyLittleInstagram.Core.DTOs;

namespace MyLittleInstagram.Core.Interfaces.Services;

public interface IJwtService
{
    public string GenerateJwtToken(UserDto user);
    public RefreshTokenDto GenerateRefreshToken(string ipAddress);
}