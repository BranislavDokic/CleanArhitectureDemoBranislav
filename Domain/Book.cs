using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public int LibraryId { get; set; }
        public LibraryModel Library { get; set; } = null!;


        public Book(int id, string title, string description, Author author, LibraryModel library)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
            Library = library;
        }

        public Book()
        {
        }
    }


}
