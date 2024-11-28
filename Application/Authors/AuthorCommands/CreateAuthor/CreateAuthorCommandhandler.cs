using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Authors.AuthorCommands.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, List<Author>>
    {
        private readonly IGenericRepositoryInterface<Author> _genericRepository;

        public CreateAuthorCommandHandler(IGenericRepositoryInterface<Author> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<List<Author>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var author = new Author
                {
                    Name = request.NewAuthor.Name,
                    Biography = request.NewAuthor.Biography
                };

                await _genericRepository.AddAsync(author);

                var allAuthors = await _genericRepository.GetAllAsync();

                return allAuthors;
            }
            catch
            {
                throw new Exception("Author not added");
            }
        }
    }
}
