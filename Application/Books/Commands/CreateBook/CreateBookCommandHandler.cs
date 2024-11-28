using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, List<Book>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;
        private readonly IGenericRepositoryInterface<Author> _authorRepository;

        public CreateBookCommandHandler(
            IGenericRepositoryInterface<Book> bookRepository,
            IGenericRepositoryInterface<Author> authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<List<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {               
                var author = await _authorRepository.GetByIdAsync(request.NewBook.AuthorId);

                if (author == null)
                {
                    throw new Exception("Author not found");
                }

               
                var book = new Book
                {
                    Title = request.NewBook.Title,
                    Description = request.NewBook.Description,
                    Author = author  
                };

                await _bookRepository.AddAsync(book);

                var allBooks = await _bookRepository.GetAllAsync();
                return allBooks;
            }
            catch (Exception ex)
            {
                throw new Exception("Book could not be added", ex);
            }
        }
    }
}
