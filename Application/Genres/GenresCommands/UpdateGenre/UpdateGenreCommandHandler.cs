using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresCommands.UpdateGenre
{
    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, string>
    {
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;

        public UpdateGenreCommandHandler(IGenericRepositoryInterface<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<string> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var genre = await _genreRepository.GetByIdAsync(request.GenreId);

                if (genre == null)
                {
                    return $"Genre with ID {request.GenreId} not found.";
                }

                genre.Name = request.NewName;

                await _genreRepository.UpdateAsync(request.GenreId, genre);

                return "Genre updated successfully.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
