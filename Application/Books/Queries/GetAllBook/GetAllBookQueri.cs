using Application.Dtos;
using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetAllBook
{
    public class GetAllBooksQuery : IRequest<OperationResult<List<BookDTO>>>
    {
    }
}
