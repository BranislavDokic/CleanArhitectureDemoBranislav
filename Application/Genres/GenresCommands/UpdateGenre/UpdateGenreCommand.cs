using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresCommands.UpdateGenre
{
    public class UpdateGenreCommand : IRequest<string>
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
