using System.Security.Cryptography;
using MediatR;
using MyLittleInstagram.Core.DTOs;
using MyLittleInstagram.Core.Interfaces.Services;
using MyLittleInstagram.CQS.Models.Queries.AccountQueries;
using Serilog;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MyLittleInstagram.Domain.Services;

public class AccountService : IAccountService
{
    private readonly IMediator _mediator;
    private IConfiguration _configuration;

    public AccountService(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        try
        {
            var upperEmail = email.ToUpperInvariant();

            var query = new GetUserByEmailQuery(upperEmail);

            var user = await _mediator.Send(query, new CancellationToken());

            return user;
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<bool> CheckPasswordByEmailAsync(LoginDto dto)
    {
        try
        {
            var user = await GetUserByEmailAsync(dto.Login);

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                var enteredPasswordHash = GetPasswordHash(dto.Password, _configuration["ApplicationVariables:Salt"]);

                if (enteredPasswordHash.Equals($@"{user.PasswordHash}"))
                {
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    private string GetPasswordHash(string password, string salt)
    {
        try
        {
            var sha1 = new SHA1CryptoServiceProvider();

            var sha1Data = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{salt}_{password}"));
            var hashedPassword = Encoding.UTF8.GetString(sha1Data);

            return hashedPassword;
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }
}