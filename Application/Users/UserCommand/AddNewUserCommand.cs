using Application.Dtos;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Users.UserCommand
{
    public class AddNewUserCommand : IRequest<OperationResult<User>>
    {
        public AddNewUserCommand(UserDTO newUser)
        {
            NewUser = newUser;
        }

        public UserDTO NewUser { get; }
    }
}
