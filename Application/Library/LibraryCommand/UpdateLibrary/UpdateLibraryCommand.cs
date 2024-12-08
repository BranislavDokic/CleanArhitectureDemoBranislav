using Domain.Result;
using MediatR;


namespace Application.Library.LibraryCommand.UpdateLibrary
{
   public class UpdateLibraryCommand : IRequest<OperationResult<bool>>
   {
        public int LibraryId { get; }
        public string NewName { get; }

        public UpdateLibraryCommand(int libraryId, string newName)
        {
            LibraryId = libraryId;
            NewName = newName;
        }
    }
}
