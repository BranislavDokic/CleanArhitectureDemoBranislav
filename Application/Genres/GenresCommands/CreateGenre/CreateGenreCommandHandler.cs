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

namespace Application.Genres.GenresCommands.CreateGenre
{
    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, OperationResult<GenreDTO>>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;
        private readonly ILogger<CreateGenreCommandHandler> _logger;

        public CreateGenreCommandHandler(IGenericRepositoryInterface<Genre> genreRepository, ILogger<CreateGenreCommandHandler> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<OperationResult<GenreDTO>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to create genre with name: {GenreName}", request.Name);

            var existingGenre = await _genreRepository.GetAllAsync();
            if (existingGenre.Any(g => g.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning("Genre with the name '{GenreName}' already exists.", request.Name);
                return OperationResult<GenreDTO>.Failure($"Genre with the name '{request.Name}' already exists.");
            }

            var genre = new Genre
            {
                Name = request.Name
            };

            try
            {
                var createdGenre = await _genreRepository.AddAsync(genre);
                var genreDto = new GenreDTO(createdGenre);

                _logger.LogInformation("Genre with name '{GenreName}' successfully created.", request.Name);
                return OperationResult<GenreDTO>.Success(genreDto, "Genre successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating genre: {GenreName}", request.Name);
                return OperationResult<GenreDTO>.Failure(ex.Message, "Failed to create genre.");
            }
        }
    }
}
