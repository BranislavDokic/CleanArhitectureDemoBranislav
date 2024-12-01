using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Genres.GenresCommands.CreateGenre
{
    public class CreateGenreCommand : IRequest<GenreDTO>
    {
        public string Name { get; set; } = string.Empty;

        public CreateGenreCommand(string name)
        {
            Name = name;
        }
    }
}
