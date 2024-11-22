using Application.Dtos;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
