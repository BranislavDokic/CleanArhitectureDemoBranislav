using Application.Dtos;
using Domain.Result;
using MediatR;


namespace Application.Books.Queries.GetAllBook
{
    public class GetAllBooksQuery : IRequest<OperationResult<List<BookDTO>>>
    {
    }
}
