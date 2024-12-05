using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.AuthorCommands.UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<OperationResult<bool>>
    {
        public int AuthorId { get; }
        public string NewName { get; }
        public string NewBiography { get; }

        public UpdateAuthorCommand(int authorId, string newName, string newBiography)
        {
            AuthorId = authorId;
            NewName = newName;
            NewBiography = newBiography;
        }
    }
}
