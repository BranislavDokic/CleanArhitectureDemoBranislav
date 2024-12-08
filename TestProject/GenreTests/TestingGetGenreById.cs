using Application.Dtos;
using Application.Genres.GenresQuery.GetGenreById;
using Application.Interfaces.Repositoryinterfaces;
using Domain.Result;
using Domain;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingGetGenreById
{
    private IGenericRepositoryInterface<Genre> _fakeGenreRepository;
    private GetGenreByIdQueryHandler _handler;
    private ILogger<GetGenreByIdQueryHandler> _fakeLogger;

    [SetUp]
    public void Setup()
    {
        _fakeGenreRepository = A.Fake<IGenericRepositoryInterface<Genre>>(); 
        _fakeLogger = A.Fake<ILogger<GetGenreByIdQueryHandler>>(); 
        _handler = new GetGenreByIdQueryHandler(_fakeGenreRepository, _fakeLogger); 
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenGenreExists()
    {
        var genreId = 1;
        var genre = new Genre
        {
            Id = genreId,
            Name = "Science Fiction",
            Books = new List<Book>
            {
                new Book { Title = "Dune" },
                new Book { Title = "Neuromancer" }
            }
        };

        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, A<Func<IQueryable<Genre>, IQueryable<Genre>>>._))
            .Returns(Task.FromResult(genre));

        var query = new GetGenreByIdQuery(genreId);
        var cancellationToken = CancellationToken.None;

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(genreId, result.Data.Id);
        Assert.AreEqual("Science Fiction", result.Data.Name);
        Assert.AreEqual(2, result.Data.BookNames.Count);
        Assert.Contains("Dune", result.Data.BookNames);
        Assert.Contains("Neuromancer", result.Data.BookNames);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenGenreNotFound()
    {
        var genreId = 1;
        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(genreId, A<Func<IQueryable<Genre>, IQueryable<Genre>>>._))
            .Returns(Task.FromResult<Genre>(null)); 

        var query = new GetGenreByIdQuery(genreId);
        var cancellationToken = CancellationToken.None;

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Genre with ID {genreId} not found.", result.ErrorMessage);
        Assert.IsNull(result.Data);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
    {
        var exceptionMessage = "Error: Database connection failed";

        A.CallTo(() => _fakeGenreRepository.GetByIdAsync(A<int>.Ignored, A<Func<IQueryable<Genre>, IQueryable<Genre>>>._))
            .Throws(new Exception(exceptionMessage)); 

        var query = new GetGenreByIdQuery(1);
        var cancellationToken = CancellationToken.None;

        var result = await _handler.Handle(query, cancellationToken);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Error: {exceptionMessage}", result.ErrorMessage);
        Assert.IsNull(result.Data);
    }
}
