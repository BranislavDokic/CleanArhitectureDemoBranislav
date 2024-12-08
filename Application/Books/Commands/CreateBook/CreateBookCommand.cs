using Application.Dtos;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<OperationResult<List<Book>>>
    {
        public CreateBookDTO NewBook { get; set; }

        public CreateBookCommand(CreateBookDTO newBook)
        {
            NewBook = newBook;
        }
    }
}
