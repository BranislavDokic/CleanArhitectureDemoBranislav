using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorCommands.UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<OperationResult<bool>>
    {
        public int AuthorId { get; }
        public string NewName { get; }
        public string NewBiography { get; }

        public UpdateAuthorCommand(int authorId, string newName, string newBiography)
        {
            AuthorId = authorId;
            NewName = newName;
            NewBiography = newBiography;
        }
    }
}
