using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryQuery.GetLibraryById
{
    public class GetLibraryByIdQuery : IRequest<LibraryDTO>
    {
        public int LibraryId { get; }

        public GetLibraryByIdQuery(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}
