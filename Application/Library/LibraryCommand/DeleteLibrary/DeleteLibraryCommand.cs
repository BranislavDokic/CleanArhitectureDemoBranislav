using Domain.Result;
using MediatR;

namespace Application.Library.LibraryCommand.DeleteLibrary
{
    public class DeleteLibraryCommand : IRequest<OperationResult<bool>>
    {
        public int LibraryId { get; }

        public DeleteLibraryCommand(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}
