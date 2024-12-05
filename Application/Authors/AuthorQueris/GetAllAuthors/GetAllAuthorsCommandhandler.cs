using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorQueris.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;

        public GetAllAuthorsQueryHandler(IGenericRepositoryInterface<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var authors = await _authorRepository.GetAllAsync();
                if (authors == null || !authors.Any())
                {
                    return OperationResult<List<Author>>.Failure("No authors found.");
                }
                return OperationResult<List<Author>>.Success(authors, "Authors retrieved successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<List<Author>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
