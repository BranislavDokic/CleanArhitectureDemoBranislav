using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
    {
        private readonly FakeDatabas _fakeDatabas;

        public DeleteBookCommandHandler(FakeDatabas fakeDatabas)
        {
            _fakeDatabas = fakeDatabas;
        }

        public Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
           
            var book = _fakeDatabas.Books.FirstOrDefault(b => b.Id == request.BookId);

            if (book == null)
            {
                
                return Task.FromResult(false);
            }

           
            _fakeDatabas.Books.Remove(book);

            
            return Task.FromResult(true);
        }
    }
}
