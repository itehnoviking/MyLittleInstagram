using MyLittleInstagram.Core.DTOs;
using MyLittleInstagram.Core.Interfaces.Services;
using System.Net;
using AutoMapper;
using Serilog;

namespace MyLittleInstagram.Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public TokenService(IJwtService jwtService, IAccountService accountService, IMapper mapper)
        {
            _jwtService = jwtService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<JwtAuthDto> GetTokenAsync(LoginDto request, string ipAddress)
        {
            var user = await _accountService.GetUserByEmailAsync(request.Login);

            if (!await _accountService.CheckPasswordByEmailAsync(request.Login, request.Password))
            {
                Log.Warning("Trying to get jwt-token with incorrect password");
                return null;
            }

            var jwtToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken(ipAddress);

            refreshToken.UserId = user.Id;

            //await _unitOfWork.RefreshTokens.AddAsync(_mapper.Map<RefreshToken>(refreshToken));

            //await _unitOfWork.Commit();

            return new JwtAuthDto(user, jwtToken, refreshToken.Token);
        }
    }
}
