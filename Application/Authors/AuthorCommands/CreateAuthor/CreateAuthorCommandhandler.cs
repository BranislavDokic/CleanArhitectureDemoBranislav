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
            _logger.LogInformation("Försöker skapa en ny författare: {AuthorName}", request.NewAuthor.Name);

            if (string.IsNullOrEmpty(request.NewAuthor.Name))
            {
                _logger.LogWarning("Författarnamn får inte vara tomt.");
                return OperationResult<List<Author>>.Failure("Författarnamn får inte vara tomt.");
            }

            if (string.IsNullOrEmpty(request.NewAuthor.Biography))
            {
                _logger.LogWarning("Författarbiografi får inte vara tom.");
                return OperationResult<List<Author>>.Failure("Författarbiografi får inte vara tom.");
            }

            try
            {
                var existingAuthor = await _genericRepository.GetAllAsync();
                var authorAlreadyExists = existingAuthor.FirstOrDefault(a => a.Name == request.NewAuthor.Name);

                if (authorAlreadyExists != null)
                {
                    _logger.LogWarning("Författare med namnet '{AuthorName}' finns redan.", request.NewAuthor.Name);
                    return OperationResult<List<Author>>.Failure($"Författare med namnet '{request.NewAuthor.Name}' finns redan.");
                }

                var author = new Author
                {
                    Name = request.NewAuthor.Name,
                    Biography = request.NewAuthor.Biography
                };

                await _genericRepository.AddAsync(author);

                var allAuthors = await _genericRepository.GetAllAsync();
                _logger.LogInformation("Författaren '{AuthorName}' skapades framgångsrikt.", request.NewAuthor.Name);
                return OperationResult<List<Author>>.Success(allAuthors, "Författare skapad.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ett fel inträffade när författaren skulle skapas: {AuthorName}", request.NewAuthor.Name);
                return OperationResult<List<Author>>.Failure($"Ett fel inträffade när författaren skulle skapas: {ex.Message}");
            }
        }
    }
}
