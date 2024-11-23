using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.AuthorQueris.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly FakeDatabas _database;

        public GetAuthorByIdQueryHandler(FakeDatabas database)
        {
            _database = database;
        }

        public Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            // Hämta författaren baserat på ID
            var author = _database.Authors.FirstOrDefault(a => a.Id == request.Id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.Id} was not found.");
            }

            return Task.FromResult(author);
        }
    }
}
