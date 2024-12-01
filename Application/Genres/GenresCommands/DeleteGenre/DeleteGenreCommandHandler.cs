using Application.Interfaces.Repositoryinterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresCommands.DeleteGenre
{
    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, bool>
    {
        private readonly IGenericRepositoryInterface<Domain.Genre> _genreRepository;

        public DeleteGenreCommandHandler(IGenericRepositoryInterface<Domain.Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<bool> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _genreRepository.DeleteAsync(request.GenreId);

                return result == "Deleted";
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Genre with ID {request.GenreId} not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the genre.", ex);
            }
        }

    }
}
