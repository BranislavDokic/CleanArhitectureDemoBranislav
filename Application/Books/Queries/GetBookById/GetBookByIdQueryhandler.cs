using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Books.Queries.GetBookById
{
    public class GetBookByIdQueryhandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;

        public GetBookByIdQueryhandler(IGenericRepositoryInterface<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book == null)
            {
                throw new KeyNotFoundException($"No book found with ID {request.Id}");
            }

            return book;
        }

    }
}
