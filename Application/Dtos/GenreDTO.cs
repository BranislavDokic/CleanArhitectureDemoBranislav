using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public GenreDTO() { }

        public List<string> BookNames { get; set; } = new();

        public GenreDTO(Domain.Genre genre)
        {
            Id = genre.Id;
            Name = genre.Name;
        }
    }
}
