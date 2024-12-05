using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
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

        public CreateGenreCommandHandler(IGenericRepositoryInterface<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<OperationResult<GenreDTO>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var existingGenre = await _genreRepository.GetAllAsync();
            if (existingGenre.Any(g => g.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
            {
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

                return OperationResult<GenreDTO>.Success(genreDto, "Genre successfully created.");
            }
            catch (Exception ex)
            {
                return OperationResult<GenreDTO>.Failure(ex.Message, "Failed to create genre.");
            }
        }
    }
}
