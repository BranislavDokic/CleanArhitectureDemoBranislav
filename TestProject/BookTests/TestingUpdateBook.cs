using Application.Books.Commands.UpdateBook;
using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;

public class TestingUpdateBook
{
    private IGenericRepositoryInterface<Book> _fakeBookRepository;
    private IGenericRepositoryInterface<Genre> _fakeGenreRepository;
    private ILogger<UpdateBookCommandHandler> _fakeLogger;
    private UpdateBookCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _fakeBookRepository = A.Fake<IGenericRepositoryInterface<Book>>();
        _fakeGenreRepository = A.Fake<IGenericRepositoryInterface<Genre>>();
        _fakeLogger = A.Fake<ILogger<UpdateBookCommandHandler>>();

        _handler = new UpdateBookCommandHandler(_fakeBookRepository, _fakeGenreRepository, _fakeLogger);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenBookIsUpdatedSuccessfully()
    {
        var bookId = 1;
        var existingBook = new Book
        {
            Id = bookId,
            Title = "Old Title",
            Description = "Old Description",
            Genres = new List<Genre> { new Genre { Name = "Old Genre" } }
        };

        var updatedBookDto = new UpdateBookDTO
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Genres = new List<string> { "Updated Genre" }
        };

        A.CallTo(() => _fakeBookRepository.GetByIdAsync(bookId, null))
            .Returns(Task.FromResult(existingBook));

        var genres = new List<Genre> { new Genre { Name = "Updated Genre" } };
        A.CallTo(() => _fakeGenreRepository.GetAllAsync())
            .Returns(Task.FromResult(genres));

        A.CallTo(() => _fakeBookRepository.UpdateAsync(bookId, A<Book>._))
       .Returns(Task.FromResult(existingBook));

        var command = new UpdateBookCommand(bookId, updatedBookDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Book successfully updated.", result.Message);
        Assert.AreEqual("Updated Title", result.Data.Title);
        Assert.AreEqual("Updated Description", result.Data.Description);
        Assert.Contains("Updated Genre", result.Data.Genres);
        A.CallTo(() => _fakeBookRepository.UpdateAsync(bookId, A<Book>._)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenBookDoesNotExist()
    {
        var bookId = 1;
        var updatedBookDto = new UpdateBookDTO
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Genres = new List<string> { "Updated Genre" }
        };

        A.CallTo(() => _fakeBookRepository.GetByIdAsync(bookId, null))
            .Returns(Task.FromResult<Book>(null));

        var command = new UpdateBookCommand(bookId, updatedBookDto);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Book with ID {bookId} not found.", result.ErrorMessage);
        A.CallTo(() => _fakeBookRepository.UpdateAsync(bookId, A<Book>._)).MustNotHaveHappened();
    }
}