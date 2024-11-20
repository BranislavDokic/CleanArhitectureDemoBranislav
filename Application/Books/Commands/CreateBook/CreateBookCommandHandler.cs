using Domain;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.CreateBook
{
    internal class CreateBookCommandHandler
    {
        private readonly FakeDatabas _fakeDatabas;

        public CreateBookCommandHandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<List<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            
            _fakeDatabas.Add(request.NewBook);
            return Task.FromResult(_fakeDatabas.Books);
        }
    }
}
