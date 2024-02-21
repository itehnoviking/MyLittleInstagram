using MediatR;
using MyLittleInstagram.Core.DTOs;

namespace MyLittleInstagram.CQS.Models.Queries.AccountQueries;

public class GetUserByEmailQuery : IRequest<UserDto>
{
    public GetUserByEmailQuery(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
}