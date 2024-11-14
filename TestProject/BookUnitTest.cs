using Application;
using Domain;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void When_Method_AddNewBook_IsCaled_ThenBookAddedToList()
        {
            Book bookToTest = new Book(1, "Branislav", "Book of Branislav");

            Book bookCreated = BookMethods.AddNewBook();

            Assert.That(bookCreated.Description, Is.EqualTo(bookToTest.Description));
        }
    }
}