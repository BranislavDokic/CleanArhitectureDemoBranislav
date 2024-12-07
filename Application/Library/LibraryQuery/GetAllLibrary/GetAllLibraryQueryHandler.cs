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

namespace Application.Library.LibraryQuery.GetAllLibrary
{
    public class GetAllLibraryQueryHandler : IRequestHandler<GetAllLibraryQuery, OperationResult<List<LibraryDTO>>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;
        private readonly ILogger<GetAllLibraryQueryHandler> _logger;

        public GetAllLibraryQueryHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository, ILogger<GetAllLibraryQueryHandler> logger)
        {
            _libraryRepository = libraryRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<LibraryDTO>>> Handle(GetAllLibraryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve all libraries.");

            try
            {
                var libraries = await _libraryRepository.GetAllAsync();

                if (libraries == null || !libraries.Any())
                {
                    _logger.LogWarning("No libraries found.");
                    return OperationResult<List<LibraryDTO>>.Failure("No libraries found.");
                }

                var libraryDTOs = libraries.Select(library => new LibraryDTO(library)).ToList();
                _logger.LogInformation("Successfully retrieved libraries.");

                return OperationResult<List<LibraryDTO>>.Success(libraryDTOs, "Libraries retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving libraries.");
                return OperationResult<List<LibraryDTO>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
