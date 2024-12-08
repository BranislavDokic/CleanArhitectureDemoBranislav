using Application.Books.Commands.CreateBook;
using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace TestProject.BookTests
{
    public class Tests
    {
        private IGenericRepositoryInterface<Book> _fakeBookRepository;
        private IGenericRepositoryInterface<Author> _fakeAuthorRepository;
        private IGenericRepositoryInterface<LibraryModel> _fakeLibraryRepository;
        private IGenericRepositoryInterface<Genre> _fakeGenreRepository;
        private ILogger<CreateBookCommandHandler> _fakeLogger;
        private CreateBookCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _fakeBookRepository = A.Fake<IGenericRepositoryInterface<Book>>();
            _fakeAuthorRepository = A.Fake<IGenericRepositoryInterface<Author>>();
            _fakeLibraryRepository = A.Fake<IGenericRepositoryInterface<LibraryModel>>();
            _fakeGenreRepository = A.Fake<IGenericRepositoryInterface<Genre>>();
            _fakeLogger = A.Fake<ILogger<CreateBookCommandHandler>>();

            _handler = new CreateBookCommandHandler(
                _fakeBookRepository,
                _fakeAuthorRepository,
                _fakeLibraryRepository,
                _fakeGenreRepository,
                _fakeLogger
            );
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenBookIsCreatedSuccessfully()
        {
            var newBookDto = new CreateBookDTO
            {
                Title = "New Book",
                AuthorName = "Existing Author",
                LibraryName = "Library 1",
                GenreNames = new List<string> { "Genre 1" },
                Description = "A description"
            };

            var authors = new List<Author> { new Author { Name = "Existing Author" } };
            var libraries = new List<LibraryModel> { new LibraryModel { Name = "Library 1" } };
            var genres = new List<Genre> { new Genre { Name = "Genre 1" } };

            A.CallTo(() => _fakeAuthorRepository.GetAllAsync()).Returns(Task.FromResult(authors));
            A.CallTo(() => _fakeLibraryRepository.GetAllAsync()).Returns(Task.FromResult(libraries));
            A.CallTo(() => _fakeGenreRepository.GetAllAsync()).Returns(Task.FromResult(genres));

            var command = new CreateBookCommand(newBookDto);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Book created successfully.", result.Message);
            A.CallTo(() => _fakeBookRepository.AddAsync(A<Book>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenAuthorDoesNotExist()
        {
            var newBookDto = new CreateBookDTO
            {
                Title = "New Book",
                AuthorName = "Non-existing Author",
                LibraryName = "Library 1",
                GenreNames = new List<string> { "Genre 1" },
                Description = "A description"
            };

            var authors = new List<Author>();
            var libraries = new List<LibraryModel> { new LibraryModel { Name = "Library 1" } };
            var genres = new List<Genre> { new Genre { Name = "Genre 1" } };

            A.CallTo(() => _fakeAuthorRepository.GetAllAsync()).Returns(Task.FromResult(authors));
            A.CallTo(() => _fakeLibraryRepository.GetAllAsync()).Returns(Task.FromResult(libraries));
            A.CallTo(() => _fakeGenreRepository.GetAllAsync()).Returns(Task.FromResult(genres));

            var command = new CreateBookCommand(newBookDto);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Author 'Non-existing Author' not found.", result.ErrorMessage);
            A.CallTo(() => _fakeBookRepository.AddAsync(A<Book>._)).MustNotHaveHappened();
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenLibraryDoesNotExist()
        {
            var newBookDto = new CreateBookDTO
            {
                Title = "New Book",
                AuthorName = "Existing Author",
                LibraryName = "Non-existing Library",
                GenreNames = new List<string> { "Genre 1" },
                Description = "A description"
            };

            var authors = new List<Author> { new Author { Name = "Existing Author" } };
            var libraries = new List<LibraryModel>();
            var genres = new List<Genre> { new Genre { Name = "Genre 1" } };

            A.CallTo(() => _fakeAuthorRepository.GetAllAsync()).Returns(Task.FromResult(authors));
            A.CallTo(() => _fakeLibraryRepository.GetAllAsync()).Returns(Task.FromResult(libraries));
            A.CallTo(() => _fakeGenreRepository.GetAllAsync()).Returns(Task.FromResult(genres));

            var command = new CreateBookCommand(newBookDto);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Library 'Non-existing Library' not found.", result.ErrorMessage);
            A.CallTo(() => _fakeBookRepository.AddAsync(A<Book>._)).MustNotHaveHappened();
        }


    }
}