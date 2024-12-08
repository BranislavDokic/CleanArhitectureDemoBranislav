using Application.Authors.AuthorCommands.CreateAuthor;
using Application.Authors.AuthorCommands.DeleteAuthor;
using Application.Authors.AuthorCommands.UpdateAuthor;
using Application.Authors.AuthorQueris.GetAllAuthors;
using Application.Authors.AuthorQueris.GetAuthorById;
using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using FakeItEasy;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;

namespace TestProject;

public class AuthorUnitTest
{
    private IGenericRepositoryInterface<Author> _fakeRepository;
    private IRequestHandler<CreateAuthorCommand, OperationResult<List<Author>>> _createHandler;
    private IRequestHandler<DeleteAuthorCommand, OperationResult<bool>> _deleteHandler;
    private IRequestHandler<UpdateAuthorCommand, OperationResult<bool>> _updateHandler;
    private IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>> _getAllAuthors;
    private IRequestHandler<GetAuthorByIdQuery, OperationResult<Author>> _getAuthorById;
    private ILogger<CreateAuthorCommandHandler> _fakeLoggerCreate;
    private ILogger<DeleteAuthorCommandHandler> _fakeLoggerDelete;
    private ILogger<UpdateAuthorCommandHandler> _fakeLoggerUpdate;
    private ILogger<GetAllAuthorsQueryHandler> _fakeLoggerGetAll;
    private ILogger<GetAuthorByIdQueryHandler> _fakeLoggerGetById;

    [SetUp]
    public void Setup()
    {
        _fakeRepository = A.Fake<IGenericRepositoryInterface<Author>>();
        var fakeMemoryCache = A.Fake<IMemoryCache>();

        _fakeLoggerCreate = A.Fake<ILogger<CreateAuthorCommandHandler>>();
        _fakeLoggerDelete = A.Fake<ILogger<DeleteAuthorCommandHandler>>();
        _fakeLoggerUpdate = A.Fake<ILogger<UpdateAuthorCommandHandler>>();
        _fakeLoggerGetAll = A.Fake<ILogger<GetAllAuthorsQueryHandler>>();
        _fakeLoggerGetById = A.Fake<ILogger<GetAuthorByIdQueryHandler>>();

        _createHandler = new CreateAuthorCommandHandler(_fakeRepository, _fakeLoggerCreate);
        _deleteHandler = new DeleteAuthorCommandHandler(_fakeRepository, _fakeLoggerDelete);
        _updateHandler = new UpdateAuthorCommandHandler(_fakeRepository, _fakeLoggerUpdate);
        _getAllAuthors = new GetAllAuthorsQueryHandler(_fakeRepository, _fakeLoggerGetAll, fakeMemoryCache);
        _getAuthorById = new GetAuthorByIdQueryHandler(_fakeRepository, _fakeLoggerGetById);
    }

    [Test]
    public async Task Handle_ShouldAddAuthor_WhenAuthorDoesNotExist()
    {
        var newAuthorDto = new AuthorDTO
        {
            Name = "New Author",
            Biography = "Biography"
        };
        var existingAuthors = new List<Author>();
        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(Task.FromResult(existingAuthors));

        var command = new CreateAuthorCommand(newAuthorDto);
        var result = await _createHandler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Författare skapad.", result.Message);
        A.CallTo(() => _fakeRepository.AddAsync(A<Author>._)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenAuthorAlreadyExists()
    {
        var existingAuthor = new Author { Name = "Existing Author", Biography = "Existing Bio" };
        var existingAuthors = new List<Author> { existingAuthor };
        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(Task.FromResult(existingAuthors));

        var duplicateAuthorDto = new AuthorDTO
        {
            Name = existingAuthor.Name,
            Biography = "Different Bio"
        };

        var command = new CreateAuthorCommand(duplicateAuthorDto);
        var result = await _createHandler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Författare med namnet '{duplicateAuthorDto.Name}' finns redan.", result.ErrorMessage);
        A.CallTo(() => _fakeRepository.AddAsync(A<Author>._)).MustNotHaveHappened();
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenAuthorExists()
    {
        var authorId = 1;
        var existingAuthor = new Author { Id = authorId, Name = "Test Author" };

        A.CallTo(() => _fakeRepository.GetByIdAsync(authorId, null)).Returns(Task.FromResult(existingAuthor));

        A.CallTo(() => _fakeRepository.DeleteAsync(authorId)).Returns(Task.FromResult("Delete successful"));

        var command = new DeleteAuthorCommand(authorId);

        var result = await _deleteHandler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Author successfully deleted.", result.Message);

        A.CallTo(() => _fakeRepository.DeleteAsync(authorId)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenAuthorNotFound()
    {
        var authorId = 1;

        A.CallTo(() => _fakeRepository.GetByIdAsync(authorId, null)).Returns(Task.FromResult<Author>(null));

        var command = new DeleteAuthorCommand(authorId);

        var result = await _deleteHandler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Author with ID {authorId} not found.", result.ErrorMessage);

        A.CallTo(() => _fakeRepository.DeleteAsync(authorId)).MustNotHaveHappened();
    }

    [Test]
    public async Task Handle_ShouldUpdateAuthor_WhenAuthorExists()
    {
        var authorId = 1;
        var existingAuthor = new Author { Id = authorId, Name = "Old Name", Biography = "Old Bio" };
        A.CallTo(() => _fakeRepository.GetByIdAsync(authorId, null)).Returns(Task.FromResult(existingAuthor));

        var updateCommand = new UpdateAuthorCommand(authorId, "New Name", "New Bio");

        var result = await _updateHandler.Handle(updateCommand, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Author successfully updated.", result.Message);

        A.CallTo(() => _fakeRepository.UpdateAsync(authorId, A<Author>.That.Matches(a => a.Name == "New Name" && a.Biography == "New Bio"))).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenAuthorNotFound_ForUpdate()
    {
        var authorId = 1;

        A.CallTo(() => _fakeRepository.GetByIdAsync(authorId, null)).Returns(Task.FromResult<Author>(null));

        var updateCommand = new UpdateAuthorCommand(authorId, "New Name", "New Bio");

        var result = await _updateHandler.Handle(updateCommand, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Operation failed", result.Message);

        A.CallTo(() => _fakeRepository.UpdateAsync(authorId, A<Author>._)).MustNotHaveHappened();
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenAuthorsExist()
    {
        var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author 1", Biography = "Biography 1" },
                new Author { Id = 2, Name = "Author 2", Biography = "Biography 2" }
            };

        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(Task.FromResult(authors));

        var query = new GetAllAuthorsQuery();

        var result = await _getAllAuthors.Handle(query, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Authors retrieved successfully.", result.Message);
        Assert.AreEqual(2, result.Data.Count);
        Assert.AreEqual("Author 1", result.Data[0].Name);
        Assert.AreEqual("Author 2", result.Data[1].Name);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenNoAuthorsFound()
    {
        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(Task.FromResult(new List<Author>()));

        var query = new GetAllAuthorsQuery();

        var result = await _getAllAuthors.Handle(query, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Operation failed", result.Message);
    }

    [Test]
    public async Task Handle_ShouldReturnSuccess_WhenAuthorFound()
    {
        var authorId = 1;
        var author = new Author { Id = authorId, Name = "John Doe" };

        A.CallTo(() => _fakeRepository.GetByIdAsync(authorId, null)).Returns(Task.FromResult(author));

        var query = new GetAuthorByIdQuery(authorId);

        var result = await _getAuthorById.Handle(query, CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual($"Successfully returned author by Id {authorId}", result.Message);
        Assert.AreEqual(author, result.Data);
    }

    [Test]
    public async Task Handle_ShouldReturnFailure_WhenAuthorNotFoundWithThatId()
    {
        var authorId = 999;

        A.CallTo(() => _fakeRepository.GetByIdAsync(authorId, null)).Returns(Task.FromResult<Author>(null));

        var query = new GetAuthorByIdQuery(authorId);

        var result = await _getAuthorById.Handle(query, CancellationToken.None);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Failure to return author by Id {authorId}", result.ErrorMessage);
    }



}
