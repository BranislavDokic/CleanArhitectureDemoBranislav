
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Author
    {
        [Range(1, int.MaxValue, ErrorMessage = "Författarens ID måste vara större än 0.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Författarnamn är obligatoriskt.")]
        [StringLength(100, ErrorMessage = "Författarnamnet får vara högst 100 tecken långt.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Författarbiografi är obligatorisk.")]
        [StringLength(1000, ErrorMessage = "Författarbiografin får vara högst 1000 tecken lång.")]
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
