using Application.Books.Commands.CreateBook;
using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Users.UserCommand
{
    public class AddNewUserCommandhandler : IRequestHandler<AddNewUserCommand, User>
    {
        private readonly FakeDatabas _fakeDatabas;

        public AddNewUserCommandhandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<User> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            User userToAdd = new()
            {
                Id = Guid.NewGuid(),
                UserName = request.NewUser.UserName,
                Password = request.NewUser.Password
            };

            _fakeDatabas.Users.Add(userToAdd);

            
            return Task.FromResult(userToAdd);
        }
    }
}
