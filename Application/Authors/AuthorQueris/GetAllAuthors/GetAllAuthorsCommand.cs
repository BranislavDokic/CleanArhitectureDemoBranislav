using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.AuthorQueris.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<List<Author>>
    {
      
    }
}
