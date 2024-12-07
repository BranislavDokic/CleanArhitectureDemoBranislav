using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Authors.AuthorQueris.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;

        public GetAllAuthorsQueryHandler(IGenericRepositoryInterface<Author> authorRepository, ILogger<GetAllAuthorsQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }
        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve all authors.");

            try
            {
                var authors = await _authorRepository.GetAllAsync();
                if (authors == null || !authors.Any())
                {
                    _logger.LogWarning("No authors found.");
                    return OperationResult<List<Author>>.Failure("No authors found.");
                }

                _logger.LogInformation("Successfully retrieved {AuthorCount} authors.", authors.Count);
                return OperationResult<List<Author>>.Success(authors, "Authors retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving authors.");
                return OperationResult<List<Author>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
