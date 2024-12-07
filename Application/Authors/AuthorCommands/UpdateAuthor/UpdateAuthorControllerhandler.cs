using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Authors.AuthorCommands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        private readonly ILogger<UpdateAuthorCommandHandler> _logger;

        public UpdateAuthorCommandHandler(IGenericRepositoryInterface<Author> authorRepository, ILogger<UpdateAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }


        public async Task<OperationResult<bool>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update author with ID: {AuthorId}", request.AuthorId);

            try
            {
                var author = await _authorRepository.GetByIdAsync(request.AuthorId);

                if (author == null)
                {
                    _logger.LogWarning("Author with ID {AuthorId} not found.", request.AuthorId);
                    return OperationResult<bool>.Failure($"Author with ID {request.AuthorId} not found.");
                }

                author.Name = request.NewName ?? author.Name;
                author.Biography = request.NewBiography ?? author.Biography;

                await _authorRepository.UpdateAsync(request.AuthorId, author);

                _logger.LogInformation("Author with ID {AuthorId} successfully updated.", request.AuthorId);
                return OperationResult<bool>.Success(true, "Author successfully updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the author with ID: {AuthorId}", request.AuthorId);
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
