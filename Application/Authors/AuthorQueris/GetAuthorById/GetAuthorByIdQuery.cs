using Domain;
using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorQueris.GetAuthorById
{
    public class GetAuthorByIdQuery : IRequest<OperationResult<Author>>
    {
        public int Id { get; }

        public GetAuthorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
