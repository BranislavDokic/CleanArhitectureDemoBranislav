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

namespace Application.Library.LibraryCommand.UpdateLibrary
{
    public class UpdateLibraryCommandHandler : IRequestHandler<UpdateLibraryCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;
        private readonly ILogger<UpdateLibraryCommandHandler> _logger;

        public UpdateLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository, ILogger<UpdateLibraryCommandHandler> logger)
        {
            _libraryRepository = libraryRepository;
            _logger = logger;
        }

        public async Task<OperationResult<bool>> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update library with ID: {LibraryId}", request.LibraryId);

            try
            {
                var library = await _libraryRepository.GetByIdAsync(request.LibraryId);

                if (library == null)
                {
                    _logger.LogWarning("Library with ID {LibraryId} not found.", request.LibraryId);
                    return OperationResult<bool>.Failure($"Library with ID {request.LibraryId} not found.");
                }

                library.Name = request.NewName;
                await _libraryRepository.UpdateAsync(request.LibraryId, library);

                _logger.LogInformation("Library with ID {LibraryId} successfully updated.", request.LibraryId);
                return OperationResult<bool>.Success(true, "Library updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the library with ID: {LibraryId}", request.LibraryId);
                return OperationResult<bool>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
