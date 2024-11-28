using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
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
        private readonly IGenericRepositoryInterface<Book> _bookRepository;
        private readonly IGenericRepositoryInterface<Author> _authorRepository;

        public UpdateBookCommandHandler(IGenericRepositoryInterface<Book> bookRepository, IGenericRepositoryInterface<Author> authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var bookToUpdate = await _bookRepository.GetByIdAsync(request.BookId);
            if (bookToUpdate == null)
            {
                throw new Exception($"Book with ID {request.BookId} not found.");
            }

            bookToUpdate.Title = request.UpdatedBook.Title;
            bookToUpdate.Description = request.UpdatedBook.Description;

            var author = await _authorRepository.GetByIdAsync(request.UpdatedBook.AuthorId);
            if (author == null)
            {
                throw new Exception($"Author with ID {request.UpdatedBook.AuthorId} not found.");
            }

            bookToUpdate.Author = author;

            await _bookRepository.UpdateAsync(bookToUpdate.Id, bookToUpdate);

            return bookToUpdate;
        }
    }
}
