using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Authors.AuthorQueris.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        private readonly ILogger<GetAuthorByIdQueryHandler> _logger;

        public GetAuthorByIdQueryHandler(IGenericRepositoryInterface<Author> authorRepository, ILogger<GetAuthorByIdQueryHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }
        public async Task<OperationResult<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve author with ID: {AuthorId}", request.Id);

            try
            {
                var author = await _authorRepository.GetByIdAsync(request.Id);

                if (author != null)
                {
                    _logger.LogInformation("Successfully returned author with ID: {AuthorId}", request.Id);
                    return OperationResult<Author>.Success(author, $"Successfully returned author by Id {request.Id}");
                }

                _logger.LogWarning("Author with ID {AuthorId} not found.", request.Id);
                return OperationResult<Author>.Failure($"Failure to return author by Id {request.Id}", "Operation Failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the author with ID: {AuthorId}", request.Id);
                return OperationResult<Author>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
