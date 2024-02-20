using MediatR;
using MyLittleInstagram.Core.DTOs;
using MyLittleInstagram.Core.Interfaces.Services;
using MyLittleInstagram.CQS.Models.Queries.AccountQueries;
using Serilog;

namespace MyLittleInstagram.Domain.Services;

public class AccountService : IAccountService
{
    private readonly IMediator _mediator;

    public AccountService(IMediator mediator)
    {
        _mediator = mediator;
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
}