using Application.Interfaces.Repositoryinterfaces;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresCommands.DeleteGenre
{
    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<Domain.Genre> _genreRepository;

        public DeleteGenreCommandHandler(IGenericRepositoryInterface<Domain.Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var genre = await _genreRepository.GetByIdAsync(request.GenreId);

                if (genre == null)
                {
                    return OperationResult<bool>.Failure($"Genre with ID {request.GenreId} not found.");
                }

                await _genreRepository.DeleteAsync(request.GenreId);

                return OperationResult<bool>.Success(true, "Genre successfully deleted.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }

    }
}
