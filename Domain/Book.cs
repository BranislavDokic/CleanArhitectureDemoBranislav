
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Domain
{
    public class Book
    {

        [Range(1, int.MaxValue, ErrorMessage = "Bokens ID måste vara större än 0.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Boktitel är obligatorisk.")]
        [StringLength(200, ErrorMessage = "Boktitel får vara högst 200 tecken lång.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Bokbeskrivning är obligatorisk.")]
        [StringLength(2000, ErrorMessage = "Bokbeskrivningen får vara högst 2000 tecken lång.")]
        public string? Description { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public int LibraryId { get; set; }
        public LibraryModel Library { get; set; } = null!;
        [JsonIgnore]
        public List<Genre> Genres { get; set; }

        public Book()
        {
        }

        public Book(int id, string title, string description, Author author, LibraryModel library)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
            Library = library;
        }

       
    }


}
