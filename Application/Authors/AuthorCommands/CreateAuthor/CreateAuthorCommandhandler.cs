using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorCommands.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, OperationResult<List<Author>>>
    {
        private readonly IGenericRepositoryInterface<Author> _genericRepository;

        public CreateAuthorCommandHandler(IGenericRepositoryInterface<Author> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<OperationResult<List<Author>>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingAuthor = await _genericRepository.GetAllAsync();
                var authorAlreadyExists = existingAuthor.FirstOrDefault(a => a.Name == request.NewAuthor.Name);

                if (authorAlreadyExists != null)
                {
                    return OperationResult<List<Author>>.Failure($"Author with the name '{request.NewAuthor.Name}' already exists.");
                }

                var author = new Author
                {
                    Name = request.NewAuthor.Name,
                    Biography = request.NewAuthor.Biography
                };

                await _genericRepository.AddAsync(author);

                var allAuthors = await _genericRepository.GetAllAsync();
                return OperationResult<List<Author>>.Success(allAuthors, "Author successfully created.");
            }
            catch (Exception ex)
            {
                return OperationResult<List<Author>>.Failure($"An error occurred while creating the author: {ex.Message}");
            }
        }
    }
}
