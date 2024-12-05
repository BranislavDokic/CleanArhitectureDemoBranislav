using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult<List<Book>>>
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

        public async Task<OperationResult<List<Book>>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var foundAuthor = await _authorRepository.GetAllAsync();
                var author = foundAuthor.FirstOrDefault(a => a.Name == request.NewBook.AuthorName);
                if (author == null)
                {
                    return OperationResult<List<Book>>.Failure($"Author '{request.NewBook.AuthorName}' not found.");
                }

                var foundLibrary = await _libraryRepository.GetAllAsync();
                var library = foundLibrary.FirstOrDefault(l => l.Name == request.NewBook.LibraryName);
                if (library == null)
                {
                    return OperationResult<List<Book>>.Failure($"Library '{request.NewBook.LibraryName}' not found.");
                }

                var foundGenres = await _genreRepository.GetAllAsync();
                var bookGenres = foundGenres.Where(g => request.NewBook.GenreNames.Contains(g.Name)).ToList();

                var missingGenres = request.NewBook.GenreNames.Except(bookGenres.Select(g => g.Name)).ToList();
                if (missingGenres.Any())
                {
                    return OperationResult<List<Book>>.Failure($"Genres not found: {string.Join(", ", missingGenres)}");
                }

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
                return OperationResult<List<Book>>.Success(allBooks, "Book created successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<List<Book>>.Failure($"An error occurred while creating the book: {ex.Message}");
            }
        }


    }
}
