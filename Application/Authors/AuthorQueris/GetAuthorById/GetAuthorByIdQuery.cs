using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.AuthorQueris.GetAuthorById
{
    public class GetAuthorByIdQuery : IRequest<OperationResult<Author>>
    {
        public int Id { get; }

        public GetAuthorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
