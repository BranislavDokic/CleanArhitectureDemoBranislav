using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Authors.AuthorCommands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        private readonly ILogger<DeleteAuthorCommandHandler> _logger;
       

        public DeleteAuthorCommandHandler(IGenericRepositoryInterface<Author> authorRepository, ILogger<DeleteAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            
        }
        public async Task<OperationResult<bool>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete author with ID: {AuthorId}", request.AuthorId);

            try
            {
                var author = await _authorRepository.GetByIdAsync(request.AuthorId);

                if (author == null)
                {
                    _logger.LogWarning("Author with ID {AuthorId} not found.", request.AuthorId);
                    return OperationResult<bool>.Failure($"Author with ID {request.AuthorId} not found.");
                }

                await _authorRepository.DeleteAsync(request.AuthorId);
                _logger.LogInformation("Author with ID {AuthorId} successfully deleted.", request.AuthorId);
                return OperationResult<bool>.Success(true, "Author successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the author with ID: {AuthorId}", request.AuthorId);
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }

        }
    }
}
