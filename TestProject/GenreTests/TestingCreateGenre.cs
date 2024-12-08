using Application.Dtos;
using Application.Genres.GenresCommands.CreateGenre;
using Application.Interfaces.Repositoryinterfaces;
using Domain.Result;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingCreateGenre
{
    private IGenericRepositoryInterface<Genre> _fakeGenreRepository;
    private IRequestHandler<CreateGenreCommand, OperationResult<GenreDTO>> _handler;
    private ILogger<CreateGenreCommandHandler> _fakeLogger;

    [SetUp]
    public void Setup()
    {
        _fakeGenreRepository = A.Fake<IGenericRepositoryInterface<Genre>>();
        _fakeLogger = A.Fake<ILogger<CreateGenreCommandHandler>>();
        _handler = new CreateGenreCommandHandler(_fakeGenreRepository, _fakeLogger);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenGenreAlreadyExists()
    {
        var genreName = "Fiction";
        var existingGenres = new List<Genre>
        {
        new Genre { Name = genreName }
        };

        A.CallTo(() => _fakeGenreRepository.GetAllAsync())
            .Returns(Task.FromResult(existingGenres));

        var command = new CreateGenreCommand(genreName); 

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess); 
        Assert.AreEqual($"Genre with the name '{genreName}' already exists.", result.ErrorMessage); 
        A.CallTo(() => _fakeGenreRepository.GetAllAsync()).MustHaveHappenedOnceExactly(); 
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenGenreIsCreated()
    {
        var genreName = "Fantasy";
        var newGenre = new Genre { Name = genreName };

        A.CallTo(() => _fakeGenreRepository.GetAllAsync())
            .Returns(Task.FromResult(new List<Genre>()));

        A.CallTo(() => _fakeGenreRepository.AddAsync(A<Genre>._))
            .Returns(Task.FromResult(newGenre));

        var command = new CreateGenreCommand(genreName); 

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess); 
        Assert.AreEqual("Genre successfully created.", result.Message); 
        Assert.AreEqual(genreName, result.Data.Name); 
        A.CallTo(() => _fakeGenreRepository.GetAllAsync()).MustHaveHappenedOnceExactly(); 
        A.CallTo(() => _fakeGenreRepository.AddAsync(A<Genre>._)).MustHaveHappenedOnceExactly();
    }
}
