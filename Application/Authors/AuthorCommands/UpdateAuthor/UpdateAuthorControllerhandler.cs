using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;

namespace Application.Authors.AuthorCommands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        public UpdateAuthorCommandHandler(IGenericRepositoryInterface<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<bool>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var author = await _authorRepository.GetByIdAsync(request.AuthorId);

                if (author == null)
                {
                    return OperationResult<bool>.Failure($"Author with ID {request.AuthorId} not found.");
                }

                author.Name = request.NewName ?? author.Name;
                author.Biography = request.NewBiography ?? author.Biography;


                await _authorRepository.UpdateAsync(request.AuthorId, author);

                return OperationResult<bool>.Success(true, $"{author} successfully updated.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
