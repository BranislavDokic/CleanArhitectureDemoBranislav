using Application.Interfaces.Repositoryinterfaces;
using Application.Library.LibraryCommand.UpdateLibrary;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingUpdateLibrary
{
    private IGenericRepositoryInterface<LibraryModel> _fakeLibraryRepository;
    private UpdateLibraryCommandHandler _handler;
    private ILogger<UpdateLibraryCommandHandler> _fakeLogger;

    [SetUp]
    public void Setup()
    {
        _fakeLibraryRepository = A.Fake<IGenericRepositoryInterface<LibraryModel>>();  
        _fakeLogger = A.Fake<ILogger<UpdateLibraryCommandHandler>>();  
        _handler = new UpdateLibraryCommandHandler(_fakeLibraryRepository, _fakeLogger);  
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenLibraryNotFound()
    {
        var libraryId = 1;
        var newName = "New Library Name";
        var query = new UpdateLibraryCommand(libraryId, newName);
        var cancellationToken = new CancellationToken();

        A.CallTo(() => _fakeLibraryRepository.GetByIdAsync(libraryId,null)).Returns(Task.FromResult<LibraryModel>(null));

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Library with ID {libraryId} not found.", result.ErrorMessage);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenLibraryIsUpdated()
    {
        var libraryId = 1;
        var newName = "Updated Library Name";
        var existingLibrary = new LibraryModel { Id = libraryId, Name = "Old Library Name" };
        var query = new UpdateLibraryCommand(libraryId, newName);
        var cancellationToken = new CancellationToken();

        A.CallTo(() => _fakeLibraryRepository.GetByIdAsync(libraryId,null)).Returns(Task.FromResult(existingLibrary));

        A.CallTo(() => _fakeLibraryRepository.UpdateAsync(libraryId, A<LibraryModel>.That.Matches(l => l.Name == newName)))
            .Returns(Task.FromResult(existingLibrary));

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Library updated successfully.", result.Message);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
    {
        var exceptionMessage = "Error: Database connection failed";
        var libraryId = 1;
        var newName = "Updated Library Name";
        var query = new UpdateLibraryCommand(libraryId, newName);
        var cancellationToken = new CancellationToken();

        A.CallTo(() => _fakeLibraryRepository.GetByIdAsync(libraryId,null)).Throws(new Exception(exceptionMessage));

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Error: {exceptionMessage}", result.ErrorMessage);
    }
}
