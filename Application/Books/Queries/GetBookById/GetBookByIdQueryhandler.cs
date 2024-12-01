using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Books.Queries.GetBookById
{
    public class GetBookByIdQueryhandler : IRequestHandler<GetBookByIdQuery, BookDTO>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;

        public GetBookByIdQueryhandler(IGenericRepositoryInterface<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookDTO> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, query => query
                .Include(b => b.Author)   // Inkludera författaren
                .Include(b => b.Library)  // Inkludera biblioteket
                .Include(b => b.Genres)   // Inkludera genrer
            );

            if (book == null)
            {
                throw new KeyNotFoundException($"No book found with ID {request.Id}");
            }

            return new BookDTO(book); // Skapa och returnera BookDTO med all relevant info
        }

    }
}
