using MediatR;
using MyLittleInstagram.Core.DTOs;

namespace MyLittleInstagram.CQS.Models.Commands.TokenCommands;

public class AddedNewRefreshTokenCommand : IRequest<bool>
{
    public AddedNewRefreshTokenCommand(RefreshTokenDto dto)
    {
        Token = dto.Token;
        Expires = dto.Expires;
        Created = dto.Created;
        CreatedByIp = dto.CreatedByIp;
        Revoked = dto.Revoked;
        ReplaceByToken = dto.ReplaceByToken;
        UserId = dto.UserId;
    }

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