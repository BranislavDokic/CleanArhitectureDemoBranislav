using Application.Genres.GenresCommands.DeleteGenre;
using Application.Interfaces.Repositoryinterfaces;
using Domain.Result;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingDeleteGenre
{
    private IGenericRepositoryInterface<Genre> _fakeGenreRepository;
    private IRequestHandler<DeleteGenreCommand, OperationResult<bool>> _handler;
    private ILogger<DeleteGenreCommandHandler> _fakeLogger;

    [SetUp]
    public void Setup()
    {
        _fakeGenreRepository = A.Fake<IGenericRepositoryInterface<Genre>>();
        _fakeLogger = A.Fake<ILogger<DeleteGenreCommandHandler>>();
        _handler = new DeleteGenreCommandHandler(_fakeGenreRepository, _fakeLogger);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenGenreNotFound()
    {
        var genreId = 1;

        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null))
            .Returns(Task.FromResult<Genre>(null)); 

        var command = new DeleteGenreCommand(genreId);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Genre with ID {genreId} not found.", result.ErrorMessage);
        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeGenreRepository.DeleteAsync(genreId)).MustNotHaveHappened();
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenGenreDeletedSuccessfully()
    {
        var genreId = 1;
        var genre = new Genre { Id = genreId, Name = "Fantasy" };

        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null))
            .Returns(Task.FromResult(genre)); 

        A.CallTo(() => _fakeGenreRepository.DeleteAsync(genreId))
               .Returns(Task.FromResult("Deleted"));

        var command = new DeleteGenreCommand(genreId); 

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Genre successfully deleted.", result.Message);
        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeGenreRepository.DeleteAsync(genreId)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
    {
        var genreId = 1;

        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null))
            .Throws(new Exception("Database error")); 

        var command = new DeleteGenreCommand(genreId); 

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("An unexpected error occurred: Database error", result.ErrorMessage);
        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, null)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeGenreRepository.DeleteAsync(genreId)).MustNotHaveHappened();
    }
}
