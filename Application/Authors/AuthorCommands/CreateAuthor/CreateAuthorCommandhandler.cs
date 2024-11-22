using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.AuthorCommands.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, List<Author>>
    {
        private readonly FakeDatabas _fakeDatabas;

        public CreateAuthorCommandHandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<List<Author>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
           
            var existingAuthor = _fakeDatabas.Authors.FirstOrDefault(a => a.Name == request.NewAuthor.Name);
            if (existingAuthor != null)
            {
                throw new InvalidOperationException("An author with the same name already exists.");
            }

           
            var newAuthor = new Author(
                id: _fakeDatabas.Authors.Count + 1,
                name: request.NewAuthor.Name,
                biography: request.NewAuthor.Biography
            );

           
            _fakeDatabas.Authors.Add(newAuthor);
            return Task.FromResult(_fakeDatabas.Authors);
        }
    }
}
