namespace MyLittleInstagram.Core.DTOs;

public class JwtAuthDto
{
    public JwtAuthDto(UserDto user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        Email = user.Email;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }

    public uint Id { get; set; }
    public string Email { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}