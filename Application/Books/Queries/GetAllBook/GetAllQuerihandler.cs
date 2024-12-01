using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Books.Queries.GetAllBook
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDTO>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;

        public GetAllBooksQueryHandler(IGenericRepositoryInterface<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDTO>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {

            var books = await _bookRepository.GetAllAsync();
            var bookDtos = books.Select(b => new BookDTO
            {
                Title = b.Title,
                Description = b.Description,
                AuthorName = b.Author?.Name ?? string.Empty  
            }).ToList();

            return bookDtos;  // Returnera


        }
    }
}
