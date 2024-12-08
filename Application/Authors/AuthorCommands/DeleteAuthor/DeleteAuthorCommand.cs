using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorCommands.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<OperationResult<bool>>
    {
        public int AuthorId { get; }

        public DeleteAuthorCommand(int authorId)
        {
            AuthorId = authorId;
        }
    }
}
