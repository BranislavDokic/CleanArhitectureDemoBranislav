using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Authors.AuthorCommands.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, OperationResult<List<Author>>>
    {
        private readonly IGenericRepositoryInterface<Author> _genericRepository;
        private readonly ILogger<CreateAuthorCommandHandler> _logger;

        public CreateAuthorCommandHandler(IGenericRepositoryInterface<Author> genericRepository, ILogger<CreateAuthorCommandHandler> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<Author>>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to create a new author: {AuthorName}", request.NewAuthor.Name);

            try
            {
                var existingAuthor = await _genericRepository.GetAllAsync();
                var authorAlreadyExists = existingAuthor.FirstOrDefault(a => a.Name == request.NewAuthor.Name);

                if (authorAlreadyExists != null)
                {
                    _logger.LogWarning("Author with the name '{AuthorName}' already exists.", request.NewAuthor.Name);
                    return OperationResult<List<Author>>.Failure($"Author with the name '{request.NewAuthor.Name}' already exists.");
                }

                var author = new Author
                {
                    Name = request.NewAuthor.Name,
                    Biography = request.NewAuthor.Biography
                };

                await _genericRepository.AddAsync(author);

                var allAuthors = await _genericRepository.GetAllAsync();
                _logger.LogInformation("Author '{AuthorName}' successfully created.", request.NewAuthor.Name);
                return OperationResult<List<Author>>.Success(allAuthors, "Author successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the author: {AuthorName}", request.NewAuthor.Name);
                return OperationResult<List<Author>>.Failure($"An error occurred while creating the author: {ex.Message}");
            }
        }
    }
}
