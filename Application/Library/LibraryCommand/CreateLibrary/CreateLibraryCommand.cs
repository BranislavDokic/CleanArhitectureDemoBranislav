using Application.Dtos;
using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
