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

        private static List<Book> allBooksFromDb = new List<Book>
        {
            new Book (1, "BranislavBook1", "Description1"),
            new Book (2, "BranislavBook2", "Description2"),
            new Book (3, "BranislavBook3", "Description3"),
            new Book (4, "BranislavBook4", "Description4"),
            new Book (5, "BranislavBook5", "Description5"),
        };

        public Book AddNewBook(Book book)
        {
            allBooksFromDb.Add(book);
            return book;
        }
    }
}
