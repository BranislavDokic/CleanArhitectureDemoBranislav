using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Users.UserQueris.GetAllUsers
{
    internal sealed class GetAllUsersQuerihandler : IRequestHandler<GetAllUsersQueri, List<User>>
    {
        private readonly FakeDatabas _fakeDatabas;

        public GetAllUsersQuerihandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<List<User>> Handle(GetAllUsersQueri request, CancellationToken cancellationToken)
        {
            List<User> allUsersFromFakeDatabas = _fakeDatabas.Users;
            return Task.FromResult(allUsersFromFakeDatabas);
        }
    }
}
