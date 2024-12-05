using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
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
    public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, OperationResult<LibraryDTO>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public CreateLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<OperationResult<LibraryDTO>> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingLibrary = await _libraryRepository.GetAllAsync();
                var libraryAlreadyExists = existingLibrary.FirstOrDefault(l => l.Name.Equals(request.NewLibrary.Name, StringComparison.OrdinalIgnoreCase));

                if (libraryAlreadyExists != null)
                {
                    return OperationResult<LibraryDTO>.Failure($"Library with the name '{request.NewLibrary.Name}' already exists.");
                }

                var library = new LibraryModel
                {
                    Name = request.NewLibrary.Name
                };

                await _libraryRepository.AddAsync(library);

                var libraryDTO = new LibraryDTO(library);

                return OperationResult<LibraryDTO>.Success(libraryDTO, "Library successfully added.");
            }
            catch (Exception ex)
            {
                return OperationResult<LibraryDTO>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
