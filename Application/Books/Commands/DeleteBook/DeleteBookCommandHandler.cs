using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;
        private readonly ILogger<DeleteBookCommandHandler> _logger;

        public DeleteBookCommandHandler(IGenericRepositoryInterface<Book> bookRepository, ILogger<DeleteBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }


        public async Task<OperationResult<bool>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete book with ID: {BookId}", request.BookId);

            try
            {
                var book = await _bookRepository.GetByIdAsync(request.BookId);

                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found.", request.BookId);
                    return OperationResult<bool>.Failure($"Book with ID {request.BookId} not found.");
                }

                await _bookRepository.DeleteAsync(request.BookId);

                _logger.LogInformation("Book with ID {BookId} successfully deleted.", request.BookId);
                return OperationResult<bool>.Success(true, "Book successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the book with ID: {BookId}", request.BookId);
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
