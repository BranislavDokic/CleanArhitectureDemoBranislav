using Application.Dtos;
using Domain.Result;
using MediatR;


namespace Application.Library.LibraryQuery.GetLibraryById
{
    public class GetLibraryByIdQuery : IRequest<OperationResult<LibraryDTO>>
    {
        public int LibraryId { get; }

        public GetLibraryByIdQuery(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}
