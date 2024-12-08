using Application.Dtos;
using Domain.Result;
using MediatR;

namespace Application.Library.LibraryCommand.CreateLibrary
{
    public class CreateLibraryCommand : IRequest<OperationResult<LibraryDTO>>
    {
        public LibraryDTO NewLibrary { get; }

        public CreateLibraryCommand(LibraryDTO newLibrary)
        {
            NewLibrary = newLibrary;
        }
    }
}
