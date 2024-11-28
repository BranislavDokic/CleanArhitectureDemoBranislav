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
        public int AuthorId { get; set; }

        public AuthorDTO Author { get; set; } = new AuthorDTO();
        public BookDTO(Book book)
        {
            Title = book.Title ?? string.Empty;
            Description = book.Description ?? string.Empty;
            AuthorId = book.Author.Id;

            Author = new AuthorDTO(book.Author);
        }

        public BookDTO() { }
    }
}
