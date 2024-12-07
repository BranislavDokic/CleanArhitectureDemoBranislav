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

namespace Application.Genres.GenresCommands.UpdateGenre
{
    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, OperationResult<string>>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;
        private readonly ILogger<UpdateGenreCommandHandler> _logger;

        public UpdateGenreCommandHandler(IGenericRepositoryInterface<Genre> genreRepository, ILogger<UpdateGenreCommandHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<OperationResult<string>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update genre with ID: {GenreId}", request.GenreId);

            try
            {
                var genre = await _genreRepository.GetByIdAsync(request.GenreId);

                if (genre == null)
                {
                    _logger.LogWarning("Genre with ID {GenreId} not found.", request.GenreId);
                    return OperationResult<string>.Failure($"Genre with ID {request.GenreId} not found.");
                }

                genre.Name = request.NewName;
                await _genreRepository.UpdateAsync(request.GenreId, genre);

                _logger.LogInformation("Genre with ID {GenreId} successfully updated to {NewName}.", request.GenreId, request.NewName);
                return OperationResult<string>.Success("Genre updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating genre with ID: {GenreId}", request.GenreId);
                return OperationResult<string>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
