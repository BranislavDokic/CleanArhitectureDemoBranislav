using Application.Dtos;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UserCommand
{
    public class AddNewUserCommand : IRequest<User>
    {
        public AddNewUserCommand(UserDTO newUser)
        {
            NewUser = newUser;
        }

        public UserDTO NewUser { get; }
    }
}
