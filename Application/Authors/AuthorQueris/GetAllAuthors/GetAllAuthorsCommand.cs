using Domain;
using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorQueris.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<OperationResult<List<Author>>>
    {
      
    }
}
