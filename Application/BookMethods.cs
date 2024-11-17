using Domain;
using Infrastructure.Database;

namespace Application
{
    public class BookMethods
    {
        private readonly FakeDatabas _fakeDatabas;

        public BookMethods(FakeDatabas fakeDatabas) 
        {  
            _fakeDatabas = fakeDatabas; 
        }
        public Book AddNewBook()
        {
            Book newbooktoadd = new Book(1, "Branislav", "Book of Branislav");
            return _fakeDatabas.AddNewBook(newbooktoadd);
        }

        public Book AddNewBook(Book newBook)
        {
            return _fakeDatabas.AddNewBook(newBook);
        }

        public Book? GetBookById(int id)
        {
            return _fakeDatabas.GetBookById(id);
        }

        public List<Book> GetAllBooks()
        {
            return _fakeDatabas.GetAllBooks();
        }

        public bool UpdateBook(Book updatedBook)
        {
            return _fakeDatabas.UpdateBook(updatedBook);
        }

        public bool DeleteBook(int id)
        {
            return _fakeDatabas.DeleteBook(id);
        }
    }
}
