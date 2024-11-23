using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, List<Book>>
    {
        private readonly FakeDatabas _fakeDatabas;

        public CreateBookCommandHandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<List<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            
            var existingAuthor = _fakeDatabas.Authors.FirstOrDefault(a => a.Id == request.NewBook.Author.Id);
            if (existingAuthor == null)
            {
                throw new KeyNotFoundException("Author not found.");
            }

           
            var newBook = new Book(
                id: _fakeDatabas.Books.Count + 1,
                title: request.NewBook.Title,
                description: request.NewBook.Description,
                author: existingAuthor
            );

            _fakeDatabas.Books.Add(newBook);

            
            return Task.FromResult(_fakeDatabas.Books);
        }
    }
}
