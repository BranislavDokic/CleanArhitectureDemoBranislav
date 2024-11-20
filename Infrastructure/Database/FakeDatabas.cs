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
        public List<Book> Books { get; private set; }
        public List<Author> Authors { get; private set; }

        public FakeDatabas()
        {
            Authors = new List<Author>
            {
                new Author(1, "Author One", "Biography of Author One"),
                new Author(2, "Author Two", "Biography of Author Two"),
                new Author(3, "Author Three", "Biography of Author Three")
            };

            Books = new List<Book>
            {
                new Book(1, "Book One", "Description One", Authors[0]),
                new Book(2, "Book Two", "Description Two", Authors[1]),
                new Book(3, "Book Three", "Description Three", Authors[2]),
                new Book(4, "Book Four", "Description Four", Authors[0]),
                new Book(5, "Book Five", "Description Five", Authors[1])
            };
        }

        
        public void Add(Book newBook)
        {
           
            var existingAuthor = Authors.FirstOrDefault(a => a.Id == newBook.Author.Id);
            if (existingAuthor == null)
            {
               
                AddAuthor(newBook.Author);
            }
            else
            {
                
                newBook.Author = existingAuthor;
            }

           
            Books.Add(newBook);
        }

        
        public void AddAuthor(Author author)
        {
            
            Authors.Add(author);
        }
    }
}
