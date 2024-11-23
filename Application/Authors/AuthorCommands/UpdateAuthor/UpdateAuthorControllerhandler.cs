using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.AuthorCommands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, bool>
    {
        private readonly FakeDatabas _fakeDatabas;

        public UpdateAuthorCommandHandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<bool> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorToUpdate = _fakeDatabas.Authors.FirstOrDefault(a => a.Id == request.AuthorId);

            if (authorToUpdate == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.AuthorId} not found.");
            }

            authorToUpdate.Name = request.NewName ?? authorToUpdate.Name;
            authorToUpdate.Biography = request.NewBiography ?? authorToUpdate.Biography;

            return Task.FromResult(true);
        }
    }
}
