using Application.Books.Queries.GetBookById;
using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Logging;


namespace TestProject
{
    public class BookUnitTest
    {
        private IGenericRepositoryInterface<Book> _fakeBookRepository;
        private IRequestHandler<GetBookByIdQuery, OperationResult<BookDTO>> _handler;
        private ILogger<GetBookByIdQueryhandler> _fakeLogger;

        [SetUp]
        public void Setup()
        {
            _fakeBookRepository = A.Fake<IGenericRepositoryInterface<Book>>();
            _fakeLogger = A.Fake<ILogger<GetBookByIdQueryhandler>>();

            _handler = new GetBookByIdQueryhandler(_fakeBookRepository, _fakeLogger);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenBookExists()
        {
            var bookId = 1;
            var book = new Book
            {
                Id = bookId,
                Title = "Book Title",
                Description = "Book Description",
                Author = new Author { Name = "Author Name" },
                Library = new LibraryModel { Name = "Library Name" },
                Genres = new List<Genre> { new Genre { Name = "Fiction" } }
            };

            var bookRepositoryMock = A.Fake<IGenericRepositoryInterface<Book>>();
            A.CallTo(() => bookRepositoryMock.GetByIdAsync(bookId, A<Func<IQueryable<Book>, IQueryable<Book>>>._))
                .Returns(Task.FromResult(book));

            var loggerMock = A.Fake<ILogger<GetBookByIdQueryhandler>>();
            var handler = new GetBookByIdQueryhandler(bookRepositoryMock, loggerMock);
            var query = new GetBookByIdQuery(bookId);

            var result = await handler.Handle(query, CancellationToken.None);

            
            Assert.AreEqual($"Successfully returned book by Id {bookId}", result.Message);  
            Assert.AreEqual(book.Title, result.Data.Title);  
            Assert.AreEqual(book.Author.Name, result.Data.AuthorName);  
            Assert.AreEqual(book.Library.Name, result.Data.LibraryName);  
            Assert.AreEqual(book.Genres[0].Name, result.Data.Genres[0]); 
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookNotFound()
        {
            var bookId = 1;

            A.CallTo(() => _fakeBookRepository.GetByIdAsync(bookId, A<Func<IQueryable<Book>, IQueryable<Book>>>._))
                .Returns(Task.FromResult<Book>(null));  

            var query = new GetBookByIdQuery(bookId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);  
            Assert.AreEqual($"Failure to return book by Id {bookId}", result.ErrorMessage);  
            A.CallTo(() => _fakeBookRepository.GetByIdAsync(bookId, A<Func<IQueryable<Book>, IQueryable<Book>>>._)).MustHaveHappenedOnceExactly();  
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
        {
            var bookId = 1;

            A.CallTo(() => _fakeBookRepository.GetByIdAsync(bookId, A<Func<IQueryable<Book>, IQueryable<Book>>>._))
                .Throws(new Exception("Database error"));

            var query = new GetBookByIdQuery(bookId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);  
            Assert.AreEqual("An error occurred: Database error", result.ErrorMessage);  
        }
    }
}