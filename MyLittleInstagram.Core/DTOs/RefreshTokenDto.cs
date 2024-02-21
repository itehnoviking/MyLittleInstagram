namespace MyLittleInstagram.Core.DTOs;

public class RefreshTokenDto
{
    public uint Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string ReplaceByToken { get; set; }
    public uint UserId { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsExpired && !IsRevoked;

}