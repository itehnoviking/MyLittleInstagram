using MyLittleInstagram.Core.DTOs;
using MyLittleInstagram.Core.Interfaces.Services;
using System.Net;
using AutoMapper;
using MediatR;
using MyLittleInstagram.CQS.Models.Commands.TokenCommands;
using Serilog;

namespace MyLittleInstagram.Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TokenService(IJwtService jwtService, IAccountService accountService, IMapper mapper, IMediator mediator)
        {
            _jwtService = jwtService;
            _accountService = accountService;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<JwtAuthDto> GetTokenAsync(LoginDto request, string ipAddress)
        {
            try
            {
                var user = await _accountService.GetUserByEmailAsync(request.Login);

                if (!await _accountService.CheckPasswordByEmailAsync(request))
                {
                    Log.Warning("Trying to get jwt-token with incorrect password");
                    return null;
                }

                var jwtToken = _jwtService.GenerateJwtToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken(ipAddress);

                refreshToken.UserId = user.Id;

                var result = await _mediator.Send(new AddedNewRefreshTokenCommand(refreshToken), new CancellationToken());

                if (!result)
                {
                    throw new Exception();
                }

                return new JwtAuthDto(user, jwtToken, refreshToken.Token);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
