//using Application.Authors.AuthorCommands.CreateAuthor;
//using Application.Authors.AuthorCommands.DeleteAuthor;
//using Application.Authors.AuthorCommands.UpdateAuthor;
//using Application.Authors.AuthorQueris.GetAllAuthors;
//using Application.Authors.AuthorQueris.GetAuthorById;
//using Application.Dtos;
//using Domain;
//using Infrastructure.Database;
//using MediatR;
//using Moq;

//namespace TestProject;

//public class AuthorUnitTest
//{
//    private FakeDatabas _fakeDatabase;
//    private IMediator _mediator;

//    [SetUp]
//    public void Setup()
//    {
//        _fakeDatabase = new FakeDatabas();
//        var mediatorMock = new Mock<IMediator>();
//        _mediator = mediatorMock.Object;
//    }

//    [Test]
//    public void CreateAuthor_ShouldAddAuthorToList()
//    {
        
//        _fakeDatabase.Authors.Clear();

//        var newAuthor = new Author(1, "New Author", "New biography");
//        var newAuthorDTO = new AuthorDTO(newAuthor);
//        var createAuthorCommand = new CreateAuthorCommand(newAuthorDTO);

//        var handler = new CreateAuthorCommandHandler(_fakeDatabase);
//        handler.Handle(createAuthorCommand, default);

//        Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(1)); 
//        Assert.That(_fakeDatabase.Authors[0].Name, Is.EqualTo("New Author")); 
//    }

//    [Test]
//    public void DeleteAuthor_ShouldRemoveAuthorFromList()
//    {
        
//        _fakeDatabase.Authors.Clear();

//        var authorToDelete = new Author(1, "Author to Delete", "Biography");
//        _fakeDatabase.Authors.Add(authorToDelete);

//        var deleteAuthorCommand = new DeleteAuthorCommand(1);
//        var handler = new DeleteAuthorCommandHandler(_fakeDatabase);
//        handler.Handle(deleteAuthorCommand, default);

//        Assert.That(_fakeDatabase.Authors.Count, Is.EqualTo(0));
//    }

//    [Test]
//    public void UpdateAuthor_ShouldModifyAuthorDetails()
//    {
//        var originalAuthor = new Author(1, "Original Name", "Original Biography");
//        _fakeDatabase.Authors.Add(originalAuthor);

//        var updateAuthorCommand = new UpdateAuthorCommand(1, "Updated Name", "Updated Biography");  
//        var handler = new UpdateAuthorCommandHandler(_fakeDatabase);

//        handler.Handle(updateAuthorCommand, default);

//        var updatedAuthor = _fakeDatabase.Authors.Find(a => a.Id == 1);
//        Assert.That(updatedAuthor.Name, Is.EqualTo("Updated Name"));
//        Assert.That(updatedAuthor.Biography, Is.EqualTo("Updated Biography"));
//    }

//    [Test]
//    public async Task GetAllAuthors_ShouldReturnAllAuthors()
//    {
//        var fakeDatabase = new FakeDatabas();  

//        fakeDatabase.Authors.Clear();

//        fakeDatabase.Authors.Add(new Author(1, "Author 1", "Biography 1"));
//        fakeDatabase.Authors.Add(new Author(2, "Author 2", "Biography 2"));

//        var getAllAuthorsQuery = new GetAllAuthorsQuery();
//        var handler = new GetAllAuthorsQueryHandler(fakeDatabase);

//        var authors = await handler.Handle(getAllAuthorsQuery, default);

//        Assert.That(authors.Count, Is.EqualTo(2));
//        Assert.That(authors.Any(a => a.Name == "Author 1"), Is.True);
//        Assert.That(authors.Any(a => a.Name == "Author 2"), Is.True);
//    }

//    [Test]
//    public async Task GetAuthorById_ShouldReturnCorrectAuthor()
//    {

//        var author = new Author(4, "Author 4", "Biography 4");  
//        _fakeDatabase.Authors.Add(author);

//        var getAuthorByIdQuery = new GetAuthorByIdQuery(4);  
//        var handler = new GetAuthorByIdQueryHandler(_fakeDatabase);

//        var result = await handler.Handle(getAuthorByIdQuery, default);

//        Assert.That(result.Name, Is.EqualTo("Author 4"));
//        Assert.That(result.Biography, Is.EqualTo("Biography 4"));
//    }

//    [Test]
//    public async Task GetAuthorById_ShouldThrowKeyNotFoundException_WhenAuthorDoesNotExist()
//    {
//        var nonExistentAuthorId = 999;
//        var getAuthorByIdQuery = new GetAuthorByIdQuery(nonExistentAuthorId);
//        var handler = new GetAuthorByIdQueryHandler(_fakeDatabase);
//        var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(getAuthorByIdQuery, default));

//        Assert.That(ex.Message, Is.EqualTo($"Author with ID {nonExistentAuthorId} was not found."));
//    }
//}
