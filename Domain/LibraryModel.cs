using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LibraryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; } = new();

        public LibraryModel() { }

        public LibraryModel(int libraryId, string name, List<Book> books)
        {
            Id = libraryId;
            Name = name;
            Books = books;
        }

    }
}
