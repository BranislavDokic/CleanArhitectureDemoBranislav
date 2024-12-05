using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UserQueris.GetAllUsers
{
    public class GetAllUsersQueri : IRequest<OperationResult<List<User>>>
    {

    }
}
