using Application.Dtos;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
