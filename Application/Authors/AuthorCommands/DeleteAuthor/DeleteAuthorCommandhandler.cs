using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Authors.AuthorCommands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;

        public DeleteAuthorCommandHandler(IGenericRepositoryInterface<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authorRepository.DeleteAsync(request.AuthorId);

                return result == "Deleted";
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Author with ID {request.AuthorId} not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the author.", ex);
            }
        }
    }
}
