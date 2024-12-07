using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CreateLibraryCommandHandler> _logger;

        public CreateLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository, ILogger<CreateLibraryCommandHandler> logger)
        {
            _libraryRepository = libraryRepository;
            _logger = logger;
        }
        public async Task<OperationResult<LibraryDTO>> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to create a new library: {LibraryName}", request.NewLibrary.Name);

            try
            {
                var existingLibraries = await _libraryRepository.GetAllAsync();
                var libraryAlreadyExists = existingLibraries.FirstOrDefault(l => l.Name.Equals(request.NewLibrary.Name, StringComparison.OrdinalIgnoreCase));

                if (libraryAlreadyExists != null)
                {
                    _logger.LogWarning("Library with the name '{LibraryName}' already exists.", request.NewLibrary.Name);
                    return OperationResult<LibraryDTO>.Failure($"Library with the name '{request.NewLibrary.Name}' already exists.");
                }

                var library = new LibraryModel
                {
                    Name = request.NewLibrary.Name
                };

                await _libraryRepository.AddAsync(library);

                var libraryDTO = new LibraryDTO(library);
                _logger.LogInformation("Library '{LibraryName}' successfully created.", request.NewLibrary.Name);

                return OperationResult<LibraryDTO>.Success(libraryDTO, "Library successfully added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the library: {LibraryName}", request.NewLibrary.Name);
                return OperationResult<LibraryDTO>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
