using Domain;


namespace Application.Dtos
{
    public class LibraryDTO
    {

        public string Name { get; set; } = string.Empty;
        public List<string> BookNames { get; set; } = new List<string>();

        public LibraryDTO(LibraryModel library)
        {
            
            Name = library.Name;
            BookNames = library.Books.Select(book => book.Title ?? "Unnamed Book").ToList();
        }

        public LibraryDTO() { }
    }
}
