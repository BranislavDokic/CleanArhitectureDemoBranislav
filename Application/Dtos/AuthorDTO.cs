using Domain;


namespace Application.Dtos
{
    public class AuthorDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;

        public AuthorDTO(Author author)
        {
            Name = author.Name;
            Biography = author.Biography;
        }

        public AuthorDTO() { }
    }
}
