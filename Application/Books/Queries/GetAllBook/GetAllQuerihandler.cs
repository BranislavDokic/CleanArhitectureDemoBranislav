using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Books.Queries.GetAllBook
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<Book>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;

        public GetAllBooksQueryHandler(IGenericRepositoryInterface<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {

            var books = await _bookRepository.GetAllAsync();
            return books;

        }
    }
}
