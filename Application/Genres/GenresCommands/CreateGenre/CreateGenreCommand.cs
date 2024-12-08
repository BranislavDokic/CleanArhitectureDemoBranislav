using Application.Dtos;
using Domain.Result;
using MediatR;


namespace Application.Genres.GenresCommands.CreateGenre
{
    public class CreateGenreCommand : IRequest<OperationResult<GenreDTO>>
    {
        public string Name { get; set; } = string.Empty;

        public CreateGenreCommand(string name)
        {
            Name = name;
        }
    }
}
