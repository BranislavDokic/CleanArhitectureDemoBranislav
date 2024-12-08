using Application.Dtos;
using Domain.Result;
using MediatR;


namespace Application.Genres.GenresQuery.GetGenreById
{
    public class GetGenreByIdQuery : IRequest<OperationResult<GenreDTO>>
    {
        public int GenreId { get; }

        public GetGenreByIdQuery(int genreId)
        {
            GenreId = genreId;
        }
    }
}
