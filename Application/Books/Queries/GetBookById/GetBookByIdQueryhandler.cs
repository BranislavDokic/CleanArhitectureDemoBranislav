using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Application.Books.Queries.GetBookById
{
    public class GetBookByIdQueryhandler : IRequestHandler<GetBookByIdQuery, OperationResult<BookDTO>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;
        private readonly ILogger<GetBookByIdQueryhandler> _logger;

        public GetBookByIdQueryhandler(IGenericRepositoryInterface<Book> bookRepository, ILogger<GetBookByIdQueryhandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }
        public async Task<OperationResult<BookDTO>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve book with ID {BookId}.", request.Id);

            try
            {
                var book = await _bookRepository.GetByIdAsync(request.Id, query => query
                    .Include(b => b.Author)
                    .Include(b => b.Library)
                    .Include(b => b.Genres)
                );

                if (book != null)
                {
                    var bookDTO = new BookDTO(book);
                    _logger.LogInformation("Successfully retrieved book with ID {BookId}.", request.Id);
                    return OperationResult<BookDTO>.Success(bookDTO, $"Successfully returned book by Id {request.Id}");
                }

                _logger.LogWarning("No book found with ID {BookId}.", request.Id);
                return OperationResult<BookDTO>.Failure($"Failure to return book by Id {request.Id}", "Operation Failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the book with ID {BookId}.", request.Id);
                return OperationResult<BookDTO>.Failure($"An error occurred: {ex.Message}");
            }
        }

    }
}
