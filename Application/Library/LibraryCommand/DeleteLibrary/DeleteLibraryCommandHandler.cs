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

namespace Application.Library.LibraryCommand.DeleteLibrary
{
    public class DeleteLibraryCommandHandler : IRequestHandler<DeleteLibraryCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;
        private readonly ILogger<DeleteLibraryCommandHandler> _logger;

        public DeleteLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository, ILogger<DeleteLibraryCommandHandler> logger)
        {
            _libraryRepository = libraryRepository;
            _logger = logger;
        }

        public async Task<OperationResult<bool>> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete library with ID: {LibraryId}", request.LibraryId);

            try
            {
                var library = await _libraryRepository.GetByIdAsync(request.LibraryId);

                if (library == null)
                {
                    _logger.LogWarning("Library with ID {LibraryId} not found.", request.LibraryId);
                    return OperationResult<bool>.Failure($"Library with ID {request.LibraryId} not found.");
                }

                await _libraryRepository.DeleteAsync(request.LibraryId);
                _logger.LogInformation("Library with ID {LibraryId} successfully deleted.", request.LibraryId);

                return OperationResult<bool>.Success(true, "Library successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the library with ID: {LibraryId}", request.LibraryId);
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
