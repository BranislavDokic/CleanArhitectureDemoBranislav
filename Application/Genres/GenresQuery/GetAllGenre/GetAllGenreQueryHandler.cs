using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Genres.GenresQuery.GetAllGenre
{
    public class GetAllGenreQueryHandler : IRequestHandler<GetAllGenreQuery, OperationResult<List<GenreDTO>>>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;
        private readonly ILogger<GetAllGenreQueryHandler> _logger;

        public GetAllGenreQueryHandler(IGenericRepositoryInterface<Genre> genreRepository, ILogger<GetAllGenreQueryHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<GenreDTO>>> Handle(GetAllGenreQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve all genres.");

            try
            {
                var genres = await _genreRepository.GetAllAsync();

                if (genres == null || !genres.Any())
                {
                    _logger.LogWarning("No genres found.");
                    return OperationResult<List<GenreDTO>>.Failure("No genres found.");
                }

                var genreDTOs = genres.Select(genre => new GenreDTO
                {
                    Id = genre.Id,
                    Name = genre.Name
                }).ToList();

                _logger.LogInformation("{GenreCount} genres retrieved successfully.", genreDTOs.Count);
                return OperationResult<List<GenreDTO>>.Success(genreDTOs, "Genres retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving genres.");
                return OperationResult<List<GenreDTO>>.Failure(ex.Message);
            }
        }
    }
}
