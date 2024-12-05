using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorCommands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;

        public DeleteAuthorCommandHandler(IGenericRepositoryInterface<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<OperationResult<bool>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var author = await _authorRepository.GetByIdAsync(request.AuthorId);

                if (author == null) 
                {
                    return OperationResult<bool>.Failure($"Author with ID {request.AuthorId} not found.");
                }

                await _authorRepository.DeleteAsync(request.AuthorId);
                return OperationResult<bool>.Success(true, "Author successfully deleted.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }

        }
    }
}
