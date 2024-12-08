
using System.ComponentModel.DataAnnotations;


namespace Domain
{
    public class Genre
    {
        [Range(1, int.MaxValue, ErrorMessage = "Genre-ID måste vara större än 0.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Genre-namn är obligatoriskt.")]
        [StringLength(100, ErrorMessage = "Genre-namnet får vara högst 100 tecken långt.")]
        public string Name { get; set; }

        public List<Book> Books { get; set; } = new();
    }
}
