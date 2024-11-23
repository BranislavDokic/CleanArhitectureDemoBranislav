using Application.Dtos;
using Domain;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<Book>
    {
        public UpdateBookCommand(int bookId, BookDTO updatedBook)
        {
            BookId = bookId;
            UpdatedBook = updatedBook;
        }

        public int BookId { get; }
        public BookDTO UpdatedBook { get; }
    }

}
