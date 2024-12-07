using Application.Interfaces.Repositoryinterfaces;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresCommands.DeleteGenre
{
    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<Domain.Genre> _genreRepository;
        private readonly ILogger<DeleteGenreCommandHandler> _logger;

        public DeleteGenreCommandHandler(IGenericRepositoryInterface<Domain.Genre> genreRepository, ILogger<DeleteGenreCommandHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<OperationResult<bool>> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete genre with ID: {GenreId}", request.GenreId);

            try
            {
                var genre = await _genreRepository.GetByIdAsync(request.GenreId);

                if (genre == null)
                {
                    _logger.LogWarning("Genre with ID {GenreId} not found.", request.GenreId);
                    return OperationResult<bool>.Failure($"Genre with ID {request.GenreId} not found.");
                }

                await _genreRepository.DeleteAsync(request.GenreId);

                _logger.LogInformation("Genre with ID {GenreId} successfully deleted.", request.GenreId);
                return OperationResult<bool>.Success(true, "Genre successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting genre with ID: {GenreId}", request.GenreId);
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }

    }
}
