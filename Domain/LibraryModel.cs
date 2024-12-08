
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain
{
    public class LibraryModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Bibliotekets ID måste vara större än 0.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bibliotekets namn är obligatoriskt.")]
        [StringLength(200, ErrorMessage = "Bibliotekets namn får vara högst 200 tecken långt.")]
        public string Name { get; set; }

        [JsonIgnore]
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
