using Domain;
using Domain.Result;
using MediatR;


namespace Application.Users.UserQueris.GetAllUsers
{
    public class GetAllUsersQueri : IRequest<OperationResult<List<User>>>
    {

    }
}
