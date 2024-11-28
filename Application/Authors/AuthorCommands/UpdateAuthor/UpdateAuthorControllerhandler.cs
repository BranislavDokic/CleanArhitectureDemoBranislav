using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;

namespace Application.Authors.AuthorCommands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, bool>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        public UpdateAuthorCommandHandler(IGenericRepositoryInterface<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<bool> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var author = await _authorRepository.GetByIdAsync(request.AuthorId);

                if (author == null)
                {
                    throw new KeyNotFoundException($"Author with ID {request.AuthorId} not found.");
                }

                author.Name = request.NewName ?? author.Name;
                author.Biography = request.NewBiography ?? author.Biography;


                await _authorRepository.UpdateAsync(request.AuthorId, author);

                return true; 
            }
            catch
            {
                throw new Exception("Author not updated");
            }
        }
    }
}
