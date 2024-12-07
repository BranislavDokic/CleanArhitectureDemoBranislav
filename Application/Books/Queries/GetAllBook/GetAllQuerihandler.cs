using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Books.Queries.GetAllBook
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<List<BookDTO>>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;
        private readonly ILogger<GetAllBooksQueryHandler> _logger;

        public GetAllBooksQueryHandler(IGenericRepositoryInterface<Book> bookRepository, ILogger<GetAllBooksQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }
        public async Task<OperationResult<List<BookDTO>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve all books.");

            try
            {
                var books = await _bookRepository.GetAllAsync();

                if (books == null || !books.Any())
                {
                    _logger.LogWarning("No books found.");
                    return OperationResult<List<BookDTO>>.Failure("No books found.");
                }

                var bookDtos = books.Select(b => new BookDTO
                {
                    Title = b.Title,
                    Description = b.Description,
                    AuthorName = b.Author?.Name ?? string.Empty
                }).ToList();

                _logger.LogInformation("Successfully retrieved all books.");
                return OperationResult<List<BookDTO>>.Success(bookDtos, "Successfully retrieved all books.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all books.");
                return OperationResult<List<BookDTO>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
