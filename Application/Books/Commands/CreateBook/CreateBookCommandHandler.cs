using Domain;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler
    {
        private readonly FakeDatabas _fakeDatabas;

        public CreateBookCommandHandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<List<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var existingauthor = _fakeDatabas.Authors.Where(author => author.Id == request.NewBook.Author.Id);
           
            return Task.FromResult(_fakeDatabas.Books);
        }
    }
}
