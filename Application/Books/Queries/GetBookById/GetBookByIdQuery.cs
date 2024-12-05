using Application.Dtos;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Result;

namespace Application.Books.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<OperationResult<BookDTO>>
    {
        public int Id { get; }

        public GetBookByIdQuery(int id)
        {
            Id = id;
        }
    }
}
