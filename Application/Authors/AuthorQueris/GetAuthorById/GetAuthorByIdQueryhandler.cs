using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Authors.AuthorQueris.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<Author>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        public GetAuthorByIdQueryHandler(IGenericRepositoryInterface<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {

            var author = await _authorRepository.GetByIdAsync(request.Id);

            if (author != null)
            {
                return OperationResult<Author>.Success(author, $"Successfully returned author by Id {request.Id}");
            }

            return OperationResult<Author>.Failure($"Failure to return author by Id {request.Id}", "Operation Failed");
        }
    }
}
