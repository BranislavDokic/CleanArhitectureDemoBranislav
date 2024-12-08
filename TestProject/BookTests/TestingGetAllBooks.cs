using Application.Books.Queries.GetAllBook;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;


public class TestingGetAllBooks
{
    private IGenericRepositoryInterface<Book> _fakeBookRepository;
    private ILogger<GetAllBooksQueryHandler> _fakeLogger;
    private GetAllBooksQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _fakeBookRepository = A.Fake<IGenericRepositoryInterface<Book>>();
        _fakeLogger = A.Fake<ILogger<GetAllBooksQueryHandler>>();

        _handler = new GetAllBooksQueryHandler(_fakeBookRepository, _fakeLogger);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenBooksExist()
    {
        var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Description = "Description 1", Author = new Author { Name = "Author 1" }},
                new Book { Id = 2, Title = "Book 2", Description = "Description 2", Author = new Author { Name = "Author 2" }}
            };

        A.CallTo(() => _fakeBookRepository.GetAllAsync()).Returns(Task.FromResult(books));

        var query = new GetAllBooksQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Successfully retrieved all books.", result.Message);
        Assert.AreEqual(2, result.Data.Count);
        Assert.AreEqual("Book 1", result.Data[0].Title);
        Assert.AreEqual("Author 1", result.Data[0].AuthorName);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenNoBooksExist()
    {
        var books = new List<Book>();

        A.CallTo(() => _fakeBookRepository.GetAllAsync()).Returns(Task.FromResult(books));

        var query = new GetAllBooksQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("No books found.", result.ErrorMessage);
    }

}
