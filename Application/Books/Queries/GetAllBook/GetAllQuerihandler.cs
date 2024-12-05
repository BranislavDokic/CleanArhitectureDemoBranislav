using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Books.Queries.GetAllBook
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<List<BookDTO>>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;

        public GetAllBooksQueryHandler(IGenericRepositoryInterface<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<List<BookDTO>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var books = await _bookRepository.GetAllAsync();

                if (books == null || !books.Any())
                {
                    return OperationResult<List<BookDTO>>.Failure("No books found.");
                }

                var bookDtos = books.Select(b => new BookDTO
                {
                    Title = b.Title,
                    Description = b.Description,
                    AuthorName = b.Author?.Name ?? string.Empty
                }).ToList();

                return OperationResult<List<BookDTO>>.Success(bookDtos, "Successfully retrieved all books.");
            }
            catch (Exception ex)
            {
                return OperationResult<List<BookDTO>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
