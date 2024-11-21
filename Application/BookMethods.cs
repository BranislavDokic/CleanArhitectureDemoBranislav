using Application.Books.Commands.CreateBook;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application
{
    public class BookMethods
    {
        private readonly IMediator _mediator;

        public BookMethods(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task AddBook(Book book)
        {
            await _mediator.Send(new CreateBookCommand(book));
        }


    }
}
