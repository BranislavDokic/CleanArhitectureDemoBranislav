using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryCommand.UpdateLibrary
{
   public class UpdateLibraryCommand : IRequest<string>
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
