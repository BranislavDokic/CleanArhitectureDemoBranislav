using Application.Dtos;
using Domain;
using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
    {
        private readonly FakeDatabas _database;

        public UpdateBookCommandHandler(FakeDatabas database)
        {
            _database = database;
        }

        public Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
           
            var bookToUpdate = _database.Books.FirstOrDefault(b => b.Id == request.BookId);
            if (bookToUpdate == null)
            {
                throw new Exception($"Book with ID {request.BookId} not found.");
            }

            
            bookToUpdate.Title = request.UpdatedBook.Title;
            bookToUpdate.Description = request.UpdatedBook.Description;

            
            var author = _database.Authors.FirstOrDefault(a => a.Id == request.UpdatedBook.AuthorId);
            if (author == null)
            {
                throw new Exception($"Author with ID {request.UpdatedBook.AuthorId} not found.");
            }

            bookToUpdate.Author = author;

            return Task.FromResult(bookToUpdate);
        }
    }
}
