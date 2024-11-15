using Application;
using Domain;
using Infrastructure.Database;

namespace TestProject
{
    public class Tests
    {
        private BookMethods _bookMethods;
        [SetUp]
        public void Setup()
        {
            FakeDatabas fakeDatabase = new FakeDatabas();

           
            _bookMethods = new BookMethods(fakeDatabase);
        }

        [Test]
        public void When_Method_AddNewBook_IsCaled_ThenBookAddedToList()
        {
            Book expectedBook = new Book(1, "Branislav", "Book of Branislav");

            // Anropa metoden AddNewBook via _bookMethods-instansen
            Book actualBook = _bookMethods.AddNewBook();

            // Kontrollera om den skapade bokens beskrivning matchar förväntad beskrivning
            Assert.That(actualBook.Description, Is.EqualTo(expectedBook.Description));
        }
    }
}