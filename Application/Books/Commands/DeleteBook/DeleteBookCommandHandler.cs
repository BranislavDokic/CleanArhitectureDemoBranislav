using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;

        public DeleteBookCommandHandler(IGenericRepositoryInterface<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }


        public async Task<OperationResult<bool>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(request.BookId);

                if (book == null)
                {
                    return OperationResult<bool>.Failure($"Book with ID {request.BookId} not found.");
                }

                await _bookRepository.DeleteAsync(request.BookId);

                return OperationResult<bool>.Success(true, "Book successfully deleted.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
