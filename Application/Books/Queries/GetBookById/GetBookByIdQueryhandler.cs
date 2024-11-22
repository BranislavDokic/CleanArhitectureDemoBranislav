using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetBookById
{
    public class GetBookByIdQueryhandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly FakeDatabas _database;

        public GetBookByIdQueryhandler(FakeDatabas database)
        {
            _database = database;
        }

        public Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = _database.Books.FirstOrDefault(b => b.Id == request.Id);

            if (book == null)
            {
                throw new KeyNotFoundException($"No book found with ID {request.Id}");
            }

            return Task.FromResult(book);
        }

    }
}
