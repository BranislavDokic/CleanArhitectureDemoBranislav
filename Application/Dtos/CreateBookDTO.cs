

namespace Application.Dtos
{
    
    
        public class CreateBookDTO
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string AuthorName { get; set; } = string.Empty;
            public string LibraryName { get; set; } = string.Empty;
            public List<string> GenreNames { get; set; } = new List<string>();

            public CreateBookDTO() { }
        }
    
}
