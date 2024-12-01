using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresQuery.GetAllGenre
{
    internal class GetAllGenreQueryHandler : IRequestHandler<GetAllGenreQuery, List<GenreDTO>>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;

        public GetAllGenreQueryHandler(IGenericRepositoryInterface<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<List<GenreDTO>> Handle(GetAllGenreQuery request, CancellationToken cancellationToken)
        {
            var genres = await _genreRepository.GetAllAsync();

            var genreDTOs = new List<GenreDTO>();
            foreach (var genre in genres)
            {
                genreDTOs.Add(new GenreDTO
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
            }

            return genreDTOs;
        }
    }
}
