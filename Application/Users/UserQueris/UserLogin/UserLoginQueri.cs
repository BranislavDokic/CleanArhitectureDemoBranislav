using Application.Dtos;
using Domain;
using MediatR;
namespace Application.Users.UserQueris.UserLogin
{
    public class UserLoginQueri : IRequest<string>
    {
        public UserLoginQueri(UserDTO loginUser)
        {
            LoginUser = loginUser;
        }

        public UserDTO LoginUser { get; }
    }
}
