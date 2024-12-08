using Application.Genres.GenresCommands.UpdateGenre;
using Application.Interfaces.Repositoryinterfaces;
using Domain.Result;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingUpdateGenre
{
    private IGenericRepositoryInterface<Genre> _fakeGenreRepository;
    private IRequestHandler<UpdateGenreCommand, OperationResult<string>> _handler;
    private ILogger<UpdateGenreCommandHandler> _fakeLogger;

    [SetUp]
    public void Setup()
    {
        _fakeGenreRepository = A.Fake<IGenericRepositoryInterface<Genre>>();
        _fakeLogger = A.Fake<ILogger<UpdateGenreCommandHandler>>();
        _handler = new UpdateGenreCommandHandler(_fakeGenreRepository, _fakeLogger);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenGenreNotFound()
    {
        var genreId = 1;
        var newName = "New Genre Name";

        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId,null))
            .Returns(Task.FromResult<Genre>(null)); 

        var command = new UpdateGenreCommand(genreId, newName);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Genre with ID {genreId} not found.", result.ErrorMessage);
        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId,null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeGenreRepository.UpdateAsync(genreId, A<Genre>.Ignored)).MustNotHaveHappened();
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenGenreUpdatedSuccessfully()
    {
        var genreId = 1;
        var newName = "Updated Genre Name";
        var existingGenre = new Genre { Id = genreId, Name = "Old Genre Name" };

        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null))
            .Returns(Task.FromResult(existingGenre)); 

        A.CallTo(() => _fakeGenreRepository.UpdateAsync(genreId, A<Genre>.That.Matches(g => g.Name == newName)))
            .Returns(Task.FromResult(existingGenre)); 

        var command = new UpdateGenreCommand(genreId, newName);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Operation successful", result.Message);
        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeGenreRepository.UpdateAsync(genreId, A<Genre>.That.Matches(g => g.Name == newName)))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
    {
        var genreId = 1;
        var newName = "New Genre Name";

        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null))
            .Throws(new Exception("Database error")); 

        var command = new UpdateGenreCommand(genreId, newName);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Error: Database error", result.ErrorMessage);
        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId,null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeGenreRepository.UpdateAsync(genreId, A<Genre>.Ignored)).MustNotHaveHappened();
    }
}
