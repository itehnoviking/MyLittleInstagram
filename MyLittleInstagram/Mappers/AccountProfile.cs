using AutoMapper;
using MyLittleInstagram.Core.DTOs;
using MyLittleInstagram.Models.Requests;

namespace MyLittleInstagram.Mappers
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AuthenticateRequest, LoginDto>()
                .ForMember(dto => dto.Login, opt => opt.MapFrom(request => request.Login))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(request => request.Password));
        }
    }
}
