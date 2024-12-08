using Application.Dtos;
using MediatR;
using Domain.Result;

namespace Application.Books.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<OperationResult<BookDTO>>
    {
        public int Id { get; }

        public GetBookByIdQuery(int id)
        {
            Id = id;
        }
    }
}
