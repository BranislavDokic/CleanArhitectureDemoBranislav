using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, List<Book>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;


        public CreateBookCommandHandler(
            IGenericRepositoryInterface<Book> bookRepository,
            IGenericRepositoryInterface<Author> authorRepository,
            IGenericRepositoryInterface<LibraryModel> libraryRepository,
            IGenericRepositoryInterface<Genre> genreRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _libraryRepository = libraryRepository;
            _genreRepository = genreRepository;
        }

        public async Task<List<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Kolla om författaren finns
                var foundAuthor = await _authorRepository.GetAllAsync();
                var author = foundAuthor.FirstOrDefault(a => a.Name == request.NewBook.AuthorName);
                if (author == null)
                {
                    throw new Exception("Author not found");
                }

                // Kolla om biblioteket finns
                var foundLibrary = await _libraryRepository.GetAllAsync();
                var library = foundLibrary.FirstOrDefault(l => l.Name == request.NewBook.LibraryName);
                if (library == null)
                {
                    throw new Exception("Library not found");
                }

                // Kolla om genrerna finns
                var foundGenres = await _genreRepository.GetAllAsync();
                var bookGenres = foundGenres.Where(g => request.NewBook.GenreNames.Contains(g.Name)).ToList();

                var missingGenres = request.NewBook.GenreNames.Except(bookGenres.Select(g => g.Name)).ToList();
                if (missingGenres.Any())
                {
                    throw new Exception($"Genres not found: {string.Join(", ", missingGenres)}");
                }

                // Skapa boken
                var book = new Book
                {
                    Title = request.NewBook.Title,
                    Description = request.NewBook.Description,
                    Author = author,
                    Library = library,
                    Genres = bookGenres
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
