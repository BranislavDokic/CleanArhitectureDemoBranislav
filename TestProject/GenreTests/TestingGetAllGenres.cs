using Application.Dtos;
using Application.Genres.GenresCommands.UpdateGenre;
using Application.Genres.GenresQuery.GetAllGenre;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingGetAllGenres
{
    private IGenericRepositoryInterface<Genre> _fakeGenreRepository;
    private IRequestHandler<GetAllGenreQuery, OperationResult<List<GenreDTO>>> _handler;
    private ILogger<GetAllGenreQueryHandler> _fakeLogger;

    [SetUp]
    public void Setup()
    {
        _fakeGenreRepository = A.Fake<IGenericRepositoryInterface<Genre>>(); 
        _fakeLogger = A.Fake<ILogger<GetAllGenreQueryHandler>>(); 
        _handler = new GetAllGenreQueryHandler(_fakeGenreRepository, _fakeLogger); 
    }

    [Test]
    public async Task Handle_ShouldReturnGenres_WhenGenresExist()
    {
        var genres = new List<Genre>
        {
            new Genre { Id = 1, Name = "Genre 1" },
            new Genre { Id = 2, Name = "Genre 2" }
        };

        A.CallTo(() => _fakeGenreRepository.GetAllAsync())
            .Returns(Task.FromResult(genres));

        var query = new GetAllGenreQuery(); 
        var cancellationToken = CancellationToken.None;

        
        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsTrue(result.IsSuccess); 
        Assert.AreEqual("Genres retrieved successfully.", result.Message); 
        Assert.AreEqual(2, result.Data.Count); 
        Assert.AreEqual("Genre 1", result.Data[0].Name); 
        Assert.AreEqual("Genre 2", result.Data[1].Name); 
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenNoGenresExist()
    {
        var genres = new List<Genre>(); 

        A.CallTo(() => _fakeGenreRepository.GetAllAsync())
            .Returns(Task.FromResult(genres));

        var query = new GetAllGenreQuery(); 
        var cancellationToken = CancellationToken.None;

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsFalse(result.IsSuccess); 
        Assert.AreEqual("Operation failed", result.Message); 
        Assert.IsNull(result.Data); 
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
    {
        var exceptionMessage = "Operation failed"; 

        A.CallTo(() => _fakeGenreRepository.GetAllAsync())
            .Throws(new System.Exception(exceptionMessage));

        var query = new GetAllGenreQuery();
        var cancellationToken = CancellationToken.None;

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(exceptionMessage, result.ErrorMessage); 
        Assert.IsNull(result.Data); 
    }
}
