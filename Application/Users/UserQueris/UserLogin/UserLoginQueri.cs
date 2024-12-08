using Application.Dtos;
using Domain.Result;
using MediatR;
namespace Application.Users.UserQueris.UserLogin
{
    public class UserLoginQueri : IRequest<OperationResult<string>>
    {
        public UserLoginQueri(UserDTO loginUser)
        {
            LoginUser = loginUser;
        }

        public UserDTO LoginUser { get; }
    }
}
