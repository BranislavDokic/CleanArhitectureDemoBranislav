using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class FakeDatabas
    {
        public List<Book>Books {  get { return allBooksFromDb; } set { allBooksFromDb = value; } }

        private  List<Book> allBooksFromDb = new List<Book>
        {
            new Book (1, "BranislavBook1", "Description1"),
            new Book (2, "BranislavBook2", "Description2"),
            new Book (3, "BranislavBook3", "Description3"),
            new Book (4, "BranislavBook4", "Description4"),
            new Book (5, "BranislavBook5", "Description5"),
            new Book (6, "MysteryBook", "A thrilling mystery novel."),
            new Book (7, "ScienceBook", "An insightful book about science."),
            new Book (8, "FantasyBook", "A fantasy adventure with dragons."),
            new Book (9, "HistoryBook", "A deep dive into historical events."),
            new Book (10, "TechBook", "A comprehensive guide to technology."),
        };

        public Book AddNewBook(Book book)
        {
            allBooksFromDb.Add(book);
            return book;
        }

        public Book? GetBookById(int id)
        {
            return allBooksFromDb.FirstOrDefault(b => b.Id == id);
        }

        public List<Book> GetAllBooks()
        {
            return allBooksFromDb;
        }

        public bool UpdateBook(Book updatedBook)
        {
            var book = allBooksFromDb.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book == null)
            {
                Console.WriteLine($"No book found with Id {updatedBook.Id}");
                return false;
            }

            
            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;

            return true;
        }

        public bool DeleteBook(int id)
        {
            var book = allBooksFromDb.FirstOrDefault(b => b.Id == id);
            if (book == null) return false;

            allBooksFromDb.Remove(book);
            return true;
        }
    }
}
