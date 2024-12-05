using Application.Dtos;
using Domain;
using Domain.Result;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<OperationResult<UpdateBookDTO>>
    {
        public UpdateBookCommand(int bookId, UpdateBookDTO updatedBook)
        {
            BookId = bookId;
            UpdatedBook = updatedBook;
        }

        public int BookId { get; }
        public UpdateBookDTO UpdatedBook { get; }
    }

}
