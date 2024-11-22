using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.AuthorCommands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly FakeDatabas _fakeDatabas;

        public DeleteAuthorCommandHandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorToDelete = _fakeDatabas.Authors.FirstOrDefault(a => a.Id == request.AuthorId);

            if (authorToDelete == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.AuthorId} not found.");
            }

            _fakeDatabas.Authors.Remove(authorToDelete);

            return Task.FromResult(true);
        }
    }
}
