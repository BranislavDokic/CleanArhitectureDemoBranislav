using Application;
using Domain;
using Infrastructure.Database;

namespace TestProject
{
    public class Tests
    {
        private FakeDatabas _fakeDatabase;
        private BookMethods _bookMethods;
        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabas();
            _bookMethods = new BookMethods(_fakeDatabase);
        }

        [Test]
        public void When_Method_AddNewBook_IsCaled_ThenBookAddedToList()
        {
            Book expectedBook = new Book(1, "Branislav", "Book of Branislav");

            Book actualBook = _bookMethods.AddNewBook();

            
            Assert.That(actualBook.Description, Is.EqualTo(expectedBook.Description));
        }

        [Test]
        public void AddNewBook_ShouldAddBookToList()
        {
            var newBook = new Book(6, "NewBook", "NewDescription");
            var addedBook = _bookMethods.AddNewBook(newBook);

            Assert.That(addedBook.Title, Is.EqualTo("NewBook"));
        }

        [Test]
        public void GetBookById_ShouldReturnCorrectBook()
        {
            var book = _bookMethods.GetBookById(1);

            Assert.That(book?.Title, Is.EqualTo("BranislavBook1"));
        }

        [Test]
        public void GetAllBooks_ShouldReturnAllBooks()
        {
            var books = _bookMethods.GetAllBooks();

            Assert.That(books.Count, Is.GreaterThanOrEqualTo(5));
        }

        [Test]
        public void UpdateBook_ShouldModifyBook()
        {
           
            var originalBook = new Book(1, "Original Title", "Original Description");
            _bookMethods.AddNewBook(originalBook);

           
            var updatedBook = new Book(1, "Updated Title", "Updated Description");
            var result = _bookMethods.UpdateBook(updatedBook);

           
            Assert.That(result, Is.True);

            
            var book = _bookMethods.GetBookById(1);
            Assert.That(book, Is.Not.Null);
            Assert.That(book.Title, Is.EqualTo("Updated Title"));
            Assert.That(book.Description, Is.EqualTo("Updated Description"));
        }

        [Test]
        public void DeleteBook_ShouldRemoveBook()
        {
            var result = _bookMethods.DeleteBook(1);

            Assert.IsTrue(result);

            var book = _bookMethods.GetBookById(1);
            Assert.IsNull(book);
        }


    }
}