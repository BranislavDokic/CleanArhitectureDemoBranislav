using Application.Interfaces.Repositoryinterfaces;
using Application.Library.LibraryQuery.GetAllLibrary;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingGetAllLibrary
{
    private IGenericRepositoryInterface<LibraryModel> _libraryRepository;
    private ILogger<GetAllLibraryQueryHandler> _logger;
    private GetAllLibraryQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _libraryRepository = A.Fake<IGenericRepositoryInterface<LibraryModel>>();
        _logger = A.Fake<ILogger<GetAllLibraryQueryHandler>>();
        _handler = new GetAllLibraryQueryHandler(_libraryRepository, _logger);
    }

    [Test]
    public async Task Handle_WhenLibrariesExist_ReturnsSuccessResult()
    {
        var libraries = new List<LibraryModel>
            {
                new LibraryModel { Name = "Library 1", Books = new List<Book> { new Book { Title = "Book 1" } } },
                new LibraryModel { Name = "Library 2", Books = new List<Book> { new Book { Title = "Book 2" } } }
            };

        A.CallTo(() => _libraryRepository.GetAllAsync()).Returns(Task.FromResult(libraries));

        var query = new GetAllLibraryQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, result.Data.Count);
        Assert.AreEqual("Library 1", result.Data[0].Name);
        Assert.AreEqual("Book 1", result.Data[0].BookNames[0]);
    }

    [Test]
    public async Task Handle_WhenNoLibrariesExist_ReturnsFailureResult()
    {
        A.CallTo(() => _libraryRepository.GetAllAsync()).Returns(Task.FromResult(new List<LibraryModel>()));

        var query = new GetAllLibraryQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("No libraries found.", result.ErrorMessage);
    }

    [Test]
    public async Task Handle_WhenExceptionOccurs_ReturnsFailureResult()
    {
        A.CallTo(() => _libraryRepository.GetAllAsync()).Throws<Exception>();

        var query = new GetAllLibraryQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.ErrorMessage.Contains("An error occurred"));
    }
}
