using Domain.Result;
using MediatR;


namespace Application.Genres.GenresCommands.UpdateGenre
{
    public class UpdateGenreCommand : IRequest<OperationResult<string>>
    {
        public int GenreId { get; }
        public string NewName { get; }

        public UpdateGenreCommand(int genreId, string newName)
        {
            GenreId = genreId;
            NewName = newName;
        }
    }
}
