using Application.Interfaces.Repositoryinterfaces;
using Application.Library.LibraryQuery.GetLibraryById;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingGetLibraryById
{
    private IGenericRepositoryInterface<LibraryModel> _libraryRepository;
    private ILogger<GetLibraryByIdQueryHandler> _logger;
    private GetLibraryByIdQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _libraryRepository = A.Fake<IGenericRepositoryInterface<LibraryModel>>();
        _logger = A.Fake<ILogger<GetLibraryByIdQueryHandler>>();
        _handler = new GetLibraryByIdQueryHandler(_libraryRepository, _logger);
    }

    [Test]
    public async Task Handle_WhenLibraryExists_ReturnsSuccessResult()
    {
        var library = new LibraryModel
        {
            Id = 1,
            Name = "Library 1",
            Books = new List<Book> { new Book { Title = "Book 1" } }
        };

        A.CallTo(() => _libraryRepository.GetByIdAsync(1, A<Func<IQueryable<LibraryModel>, IQueryable<LibraryModel>>>._))
            .Returns(Task.FromResult(library));

        var query = new GetLibraryByIdQuery(1);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Library 1", result.Data.Name);
        Assert.AreEqual(1, result.Data.BookNames.Count);
        Assert.AreEqual("Book 1", result.Data.BookNames[0]);
    }

    [Test]
    public async Task Handle_WhenLibraryNotFound_ReturnsFailureResult()
    {
        A.CallTo(() => _libraryRepository.GetByIdAsync(1, A<Func<IQueryable<LibraryModel>, IQueryable<LibraryModel>>>._))
            .Returns(Task.FromResult<LibraryModel>(null));

        var query = new GetLibraryByIdQuery(1);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Library with ID 1 not found.", result.ErrorMessage);
    }

    [Test]
    public async Task Handle_WhenExceptionOccurs_ReturnsFailureResult()
    {
        A.CallTo(() => _libraryRepository.GetByIdAsync(1, A<Func<IQueryable<LibraryModel>, IQueryable<LibraryModel>>>._))
            .Throws<Exception>();

        var query = new GetLibraryByIdQuery(1);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.ErrorMessage.Contains("Error:"));
    }
}
