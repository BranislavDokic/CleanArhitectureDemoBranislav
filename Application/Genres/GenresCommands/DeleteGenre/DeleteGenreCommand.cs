using Domain.Result;
using MediatR;


namespace Application.Genres.GenresCommands.DeleteGenre
{
    public class DeleteGenreCommand : IRequest<OperationResult<bool>>
    {
        public int GenreId { get; set; }

        public DeleteGenreCommand(int genreId)
        {
            GenreId = genreId;
        }
    }
}
