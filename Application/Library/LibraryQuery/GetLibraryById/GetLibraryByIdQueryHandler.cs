using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryQuery.GetLibraryById
{
    public class GetLibraryByIdQueryHandler : IRequestHandler<GetLibraryByIdQuery, OperationResult<LibraryDTO>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;
        private readonly ILogger<GetLibraryByIdQueryHandler> _logger;

        public GetLibraryByIdQueryHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository, ILogger<GetLibraryByIdQueryHandler> logger)
        {
            _libraryRepository = libraryRepository;
            _logger = logger;
        }
        public async Task<OperationResult<LibraryDTO>> Handle(GetLibraryByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve library with ID: {LibraryId}", request.LibraryId);

            try
            {
                var library = await _libraryRepository.GetByIdAsync(
                    request.LibraryId,
                    query => query.Include(l => l.Books));

                if (library == null)
                {
                    _logger.LogWarning("Library with ID {LibraryId} not found.", request.LibraryId);
                    return OperationResult<LibraryDTO>.Failure($"Library with ID {request.LibraryId} not found.");
                }

                var libraryDTO = new LibraryDTO(library)
                {
                    BookNames = library.Books.Select(b => b.Title).ToList()
                };

                _logger.LogInformation("Library with ID {LibraryId} successfully retrieved.", request.LibraryId);
                return OperationResult<LibraryDTO>.Success(libraryDTO, "Library retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the library with ID: {LibraryId}", request.LibraryId);
                return OperationResult<LibraryDTO>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
