using Application.Dtos;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryQuery.GetAllLibrary
{
    public class GetAllLibraryQuery : IRequest<OperationResult<List<LibraryDTO>>>
    {
    }
}
