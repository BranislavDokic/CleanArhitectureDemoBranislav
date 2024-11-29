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
        public string Name { get; set; } = string.Empty;

        public LibraryDTO() { }

        public LibraryDTO(LibraryModel library)
        {
            Name = library.Name;
        }
    }
}
