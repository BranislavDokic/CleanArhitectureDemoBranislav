using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresCommands.DeleteGenre
{
    public class DeleteGenreCommand : IRequest<bool>
    {
        public int GenreId { get; set; }

        public DeleteGenreCommand(int genreId)
        {
            GenreId = genreId;
        }
    }
}
