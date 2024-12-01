using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresCommands.CreateGenre
{
    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreDTO>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;

        public CreateGenreCommandHandler(IGenericRepositoryInterface<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<GenreDTO> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
          
            var genre = new Genre
            {
                Name = request.Name
            };

          
            var createdGenre = await _genreRepository.AddAsync(genre);

            
            return new GenreDTO(createdGenre);
        }
    }
}
