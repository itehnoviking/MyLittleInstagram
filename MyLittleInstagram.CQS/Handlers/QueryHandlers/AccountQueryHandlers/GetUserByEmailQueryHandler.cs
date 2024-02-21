using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyLittleInstagram.Core.DTOs;
using MyLittleInstagram.CQS.Models.Queries.AccountQueries;
using MyLittleInstagram.Data;
using Serilog;

namespace MyLittleInstagram.CQS.Handlers.QueryHandlers.AccountQueryHandlers;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
{
    private readonly MyLittleInstagramContext _database;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(MyLittleInstagramContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _database.Users
                .AsNoTracking()
                .Where(user => user.NormalizedEmail.Equals(request.Email))
                .Select(user => _mapper.Map<UserDto>(user))
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new ArgumentNullException();
            }

            return user;
        }
        catch (Exception ex)
        {
            Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }

    }
}