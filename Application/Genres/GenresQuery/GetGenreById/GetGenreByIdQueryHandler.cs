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

namespace Application.Genres.GenresQuery.GetGenreById
{
    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, OperationResult<GenreDTO>>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;
        private readonly ILogger<GetGenreByIdQueryHandler> _logger;

        public GetGenreByIdQueryHandler(IGenericRepositoryInterface<Genre> genreRepository, ILogger<GetGenreByIdQueryHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<OperationResult<GenreDTO>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve genre with ID: {GenreId}", request.GenreId);

            try
            {
                var genre = await _genreRepository.GetByIdAsync(
                    request.GenreId,
                    query => query.Include(g => g.Books));

                if (genre == null)
                {
                    _logger.LogWarning("Genre with ID {GenreId} not found.", request.GenreId);
                    return OperationResult<GenreDTO>.Failure($"Genre with ID {request.GenreId} not found.");
                }

                var bookNames = genre.Books.Select(b => b.Title).ToList();

                var genreDTO = new GenreDTO
                {
                    Id = genre.Id,
                    Name = genre.Name,
                    BookNames = bookNames
                };

                _logger.LogInformation("Genre with ID {GenreId} retrieved successfully.", request.GenreId);
                return OperationResult<GenreDTO>.Success(genreDTO, "Genre retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving genre with ID: {GenreId}", request.GenreId);
                return OperationResult<GenreDTO>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
