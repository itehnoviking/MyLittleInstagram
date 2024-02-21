using AutoMapper;
using MyLittleInstagram.CQS.Models.Commands.TokenCommands;
using MyLittleInstagram.Data.Entities;

namespace MyLittleInstagram.Mappers;

public class TokenProfile : Profile
{
    public TokenProfile()
    {
        CreateMap<AddedNewRefreshTokenCommand, RefreshToken>()
            .ForMember(entity => entity.Token, opt => opt.MapFrom(command => command.Token))
            .ForMember(entity => entity.Expires, opt => opt.MapFrom(command => command.Expires))
            .ForMember(entity => entity.Created, opt => opt.MapFrom(command => command.Created))
            .ForMember(entity => entity.CreatedByIp, opt => opt.MapFrom(command => command.CreatedByIp))
            .ForMember(entity => entity.Revoked, opt => opt.MapFrom(command => command.Revoked))
            .ForMember(entity => entity.ReplaceByToken, opt => opt.MapFrom(command => command.ReplaceByToken))
            .ForMember(entity => entity.UserId, opt => opt.MapFrom(command => command.UserId));
    }
}