using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class BookDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string LibraryName { get; set; } = string.Empty;
        public int LibraryId { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public List<string> GenreNames { get; set; } = new List<string>();

        public LibraryDTO Library { get; set; } = new LibraryDTO();
        public AuthorDTO Author { get; set; } = new AuthorDTO();
        public BookDTO(Book book)
        {
            Title = book.Title ?? string.Empty;
            Description = book.Description ?? string.Empty;
            AuthorName = book.Author?.Name ?? string.Empty;
            AuthorId = book.AuthorId;
            Author = new AuthorDTO(book.Author);
            LibraryName = book.Library?.Name ?? string.Empty;
            LibraryId = book.LibraryId;
            Library = new LibraryDTO(book.Library);
            Genres = book.Genres.Select(g => g.Name).ToList();
        }

        public BookDTO() { }
    }
}
