using AutoMapper;
using MediatR;
using MyLittleInstagram.CQS.Models.Commands.TokenCommands;
using MyLittleInstagram.Data;
using MyLittleInstagram.Data.Entities;
using Serilog;

namespace MyLittleInstagram.CQS.Handlers.CommandHandlers.TokenCommandHandlers;

public class AddedNewRefreshTokenCommandHandler : IRequestHandler<AddedNewRefreshTokenCommand, bool>
{
    private readonly MyLittleInstagramContext _database;
    private readonly IMapper _mapper;

    public AddedNewRefreshTokenCommandHandler(MyLittleInstagramContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<bool> Handle(AddedNewRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var newRefreshToken = _mapper.Map<RefreshToken>(command);

            await _database.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
            await _database.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");

            return false;
        }
    }
}