using Application.Dtos;
using Domain.Result;
using MediatR;


namespace Application.Genres.GenresQuery.GetAllGenre
{
    public class GetAllGenreQuery : IRequest<OperationResult<List<GenreDTO>>>
    {

    }
}
