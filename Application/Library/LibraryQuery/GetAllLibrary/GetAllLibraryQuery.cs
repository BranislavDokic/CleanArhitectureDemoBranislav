using Application.Dtos;
using Domain.Result;
using MediatR;


namespace Application.Library.LibraryQuery.GetAllLibrary
{
    public class GetAllLibraryQuery : IRequest<OperationResult<List<LibraryDTO>>>
    {
    }
}
