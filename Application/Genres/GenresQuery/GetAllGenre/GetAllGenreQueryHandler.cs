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

namespace Application.Genres.GenresQuery.GetAllGenre
{
    internal class GetAllGenreQueryHandler : IRequestHandler<GetAllGenreQuery, OperationResult<List<GenreDTO>>>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;

        public GetAllGenreQueryHandler(IGenericRepositoryInterface<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<OperationResult<List<GenreDTO>>> Handle(GetAllGenreQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var genres = await _genreRepository.GetAllAsync();

                if (genres == null || !genres.Any())
                {
                    return OperationResult<List<GenreDTO>>.Failure("No genres found.");
                }

                var genreDTOs = genres.Select(genre => new GenreDTO
                {
                    Id = genre.Id,
                    Name = genre.Name
                }).ToList();

                return OperationResult<List<GenreDTO>>.Success(genreDTOs, "Genres retrieved successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<List<GenreDTO>>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
