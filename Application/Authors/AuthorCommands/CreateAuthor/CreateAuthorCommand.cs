using Application.Dtos;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorCommands.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<OperationResult<List<Author>>>
    {
        public CreateAuthorCommand(AuthorDTO newAuthor)
        {
            NewAuthor = newAuthor;
        }

        public AuthorDTO NewAuthor { get; }
    }
}
