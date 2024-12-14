using Application.Dtos;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Users.UserCommand
{
    public class AddNewUserCommand : IRequest<OperationResult<IdentityResult>>
    {
        public AddNewUserCommand(UserDTO newUser)
        {
            NewUser = newUser;
        }

        public UserDTO NewUser { get; }
    }
}
