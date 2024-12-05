using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
