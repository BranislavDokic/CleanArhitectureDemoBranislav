using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresQuery.GetGenreById
{
    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreDTO>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;

        public GetGenreByIdQueryHandler(IGenericRepositoryInterface<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<GenreDTO> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.GetByIdAsync(
                request.GenreId,
                query => query.Include(g => g.Books));

            if (genre == null)
            {
                throw new KeyNotFoundException($"Genre with ID {request.GenreId} not found.");
            }

            var bookNames = genre.Books.Select(b => b.Title).ToList();

            return new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name,
                BookNames = bookNames
            };
        }
    }
}
