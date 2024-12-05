using Application.Dtos;
using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.AuthorCommands.CreateAuthor
{
    public class CreateAuthorCommand : IRequest<OperationResult<List<Author>>>
    {
        public CreateAuthorCommand(AuthorDTO newAuthor)
        {
            NewAuthor = newAuthor;
        }

        public AuthorDTO NewAuthor { get; }
    }
}
