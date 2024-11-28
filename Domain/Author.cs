using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class Author
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Biography { get; set; }

        [JsonIgnore]
        public List<Book> Books { get; set; } = new();

        public Author(int id, string name, string biography)
        {
            Id = id;
            Name = name;
            Biography = biography;
            Books = new List<Book>();
        }

        public Author() { }
    }
}
