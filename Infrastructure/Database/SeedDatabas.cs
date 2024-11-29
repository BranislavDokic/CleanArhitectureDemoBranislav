using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public static class SeedDatabas
    {
        public static void Initialize(IServiceProvider serviceProvider, RealDatabase context)
        {
            if (context.Authors.Any() || context.Books.Any() || context.Libraries.Any() || context.Genres.Any())
            {
                return;   
            }

           
            var genres = new Genre[]
            {
            new Genre { Name = "Fiction" },
            new Genre { Name = "Fantasy" },
            new Genre { Name = "Non-fiction" }
            };

            context.Genres.AddRange(genres);
            context.SaveChanges();

          
            var libraries = new LibraryModel[]
            {
            new LibraryModel { Name = "City Library" },
            new LibraryModel { Name = "National Library" }
            };

            context.Libraries.AddRange(libraries);
            context.SaveChanges();

          
            var authors = new Author[]
            {
            new Author { Name = "J.K. Rowling", Biography = "British author" },
            new Author { Name = "George R.R. Martin", Biography = "American novelist" }
            };

            context.Authors.AddRange(authors);
            context.SaveChanges();

           
            var books = new Book[]
            {
            new Book
            {
                Title = "Harry Potter and the Philosopher's Stone",
                Description = "Fantasy novel about a young wizard.",
                AuthorId = authors[0].Id,
                LibraryId = libraries[0].Id,
                Genres = new List<Genre> { genres[1], genres[0] } 
            },
            new Book
            {
                Title = "A Game of Thrones",
                Description = "Epic fantasy novel with political intrigue.",
                AuthorId = authors[1].Id,
                LibraryId = libraries[1].Id,
                Genres = new List<Genre> { genres[1], genres[0] } 
            }
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
