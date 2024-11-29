using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class LibraryDTO
    {
        public int LibraryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<BookDTO> Books { get; set; } = new();

        public LibraryDTO() { }

        public LibraryDTO(LibraryModel library)
        {
            LibraryId = library.Id;
            Name = library.Name;
            Books = library.Books.Select(book => new BookDTO(book)).ToList(); 
        }
    }
}
