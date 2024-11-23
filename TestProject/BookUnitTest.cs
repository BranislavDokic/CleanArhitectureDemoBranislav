using Application;
using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Queries.GetAllBook;
using Application.Books.Queries.GetBookById;
using Application.Dtos;
using Domain;
using Infrastructure.Database;
using MediatR;
using Moq;
using WebAPI.Controllers;

namespace TestProject
{
    public class Tests
    {
        private FakeDatabas _fakeDatabase;
        private IMediator _mediator;
        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabas();
            var mediatorMock = new Mock<IMediator>();
            _mediator = mediatorMock.Object;
        }


        [Test]
        public async Task Handle_ShouldReturnAllBooks()
        {
           
            var fakeDatabase = new FakeDatabas();
            fakeDatabase.Books.Clear();
            fakeDatabase.Books.AddRange(new List<Book>
            {
                new Book(1, "Book One", "Description One", fakeDatabase.Authors.First()), 
                new Book(2, "Book Two", "Description Two", fakeDatabase.Authors.Skip(1).First())  
            });

            var handler = new GetAllBooksQueryHandler(fakeDatabase);
            var query = new GetAllBooksQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result, "Result should not be null.");
            Assert.AreEqual(2, result.Count, "The result should contain 2 books.");
            Assert.IsTrue(result.Any(b => b.Title == "Book One"), "The result should contain a book with the title 'Book One'.");
            Assert.IsTrue(result.Any(b => b.Title == "Book Two"), "The result should contain a book with the title 'Book Two'.");
        }

        [Test]
        public async Task Handle_ShouldAddNewBook_WhenAuthorExists()
        {
            var fakeDatabase = new FakeDatabas();

            var author = fakeDatabase.Authors.First(a => a.Id == 1);

            var newBook = new Book(15, "New Book", "Description of the new book", author);
            var command = new CreateBookCommand(newBook);
            var handler = new CreateBookCommandHandler(fakeDatabase);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(6, fakeDatabase.Books.Count, "Database should contain one more book.");
            var addedBook = fakeDatabase.Books.FirstOrDefault(b => b.Title == "New Book");
            Assert.IsNotNull(addedBook, "New book should have been added to the database.");
        }

        [Test]
        public async Task Handle_ShouldReturnTrue_WhenBookExists()
        {

            var fakeDatabase = new FakeDatabas();
            var bookToRemove = fakeDatabase.Books.First(b => b.Id == 1); 
            var handler = new DeleteBookCommandHandler(fakeDatabase);
            var command = new DeleteBookCommand(bookToRemove.Id);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result, "Handler should return true when the book exists.");
            Assert.That(fakeDatabase.Books, Does.Not.Contain(bookToRemove), "Book should be removed from the database.");
        }

        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
        {
            var fakeDatabase = new FakeDatabas();
            var nonExistentBookId = 999; 
            var query = new GetBookByIdQuery(nonExistentBookId);
            var handler = new GetBookByIdQueryhandler(fakeDatabase);

            Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Test]
        public void UpdateBook_ShouldModifyBookDetails()
        {
            
            var fakeDatabase = new FakeDatabas();

            var originalBook = new Book(6, "Original Title", "Original Description", new Author(99, "Original Author", "Original Biography"));
            fakeDatabase.Books.Add(originalBook);

            var updatedBookDto = new BookDTO
            {
                Title = "Updated Title",
                Description = "Updated Description",
                AuthorId = 1 
            };

            var handler = new UpdateBookCommandHandler(fakeDatabase);
            var updateCommand = new UpdateBookCommand(6, updatedBookDto);

            var result = handler.Handle(updateCommand, default).Result;

            Assert.That(result.Title, Is.EqualTo("Updated Title"));
            Assert.That(result.Description, Is.EqualTo("Updated Description"));
            Assert.That(result.Author.Name, Is.EqualTo("Author One")); 
        }

    }
}