using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryCommand.CreateLibrary
{
    public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, LibraryDTO>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public CreateLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<LibraryDTO> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var library = new LibraryModel
                {
                    Name = request.NewLibrary.Name
                };

                await _libraryRepository.AddAsync(library);

                return new LibraryDTO(library);
            }
            catch (Exception)
            {
                throw new Exception("Library not added");
            }
        }
    }
}
