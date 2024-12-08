

namespace Application.Dtos
{
    public class UpdateBookDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<string>? Genres { get; set; }
    }
}
