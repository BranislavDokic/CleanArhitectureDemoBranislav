using Application.Interfaces.Repositoryinterfaces;
using Application.Library.LibraryCommand.DeleteLibrary;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System.Net;

namespace TestProject;

public class TestingDeleteLibrary
{
    private IGenericRepositoryInterface<LibraryModel> _fakeLibraryRepository;
    private DeleteLibraryCommandHandler _handler;
    private ILogger<DeleteLibraryCommandHandler> _fakeLogger;

    [SetUp]
    public void Setup()
    {
        _fakeLibraryRepository = A.Fake<IGenericRepositoryInterface<LibraryModel>>();  
        _fakeLogger = A.Fake<ILogger<DeleteLibraryCommandHandler>>(); 
        _handler = new DeleteLibraryCommandHandler(_fakeLibraryRepository, _fakeLogger);  
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenLibraryNotFound()
    {
        var libraryId = 1;
        var query = new DeleteLibraryCommand(libraryId);
        var cancellationToken = new CancellationToken();

        A.CallTo(() => _fakeLibraryRepository.GetByIdAsync(libraryId,null)).Returns(Task.FromResult<LibraryModel>(null));

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Library with ID {libraryId} not found.", result.ErrorMessage);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenLibraryIsDeleted()
    {
        var libraryId = 1;
        var existingLibrary = new LibraryModel { Id = libraryId, Name = "Test Library" };
        var query = new DeleteLibraryCommand(libraryId);
        var cancellationToken = new CancellationToken();

        A.CallTo(() => _fakeLibraryRepository.GetByIdAsync(libraryId,null)).Returns(Task.FromResult(existingLibrary));

        A.CallTo(() => _fakeLibraryRepository.DeleteAsync(libraryId))
               .Returns(Task.FromResult("Deleted"));

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Library successfully deleted.", result.Message);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
    {
        var exceptionMessage = "Error: Database connection failed";
        var libraryId = 1;
        var query = new DeleteLibraryCommand(libraryId);
        var cancellationToken = new CancellationToken();

        A.CallTo(() => _fakeLibraryRepository.GetByIdAsync(libraryId,null)).Throws(new Exception(exceptionMessage));

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"An unexpected error occurred: {exceptionMessage}", result.ErrorMessage);
    }
}
