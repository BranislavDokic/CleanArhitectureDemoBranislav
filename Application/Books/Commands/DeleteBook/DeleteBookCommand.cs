using Domain.Result;
using MediatR;


namespace Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<OperationResult<bool>>
    {
        public int BookId { get; }

        public DeleteBookCommand(int bookId)
        {
            BookId = bookId;
        }
    }
}
