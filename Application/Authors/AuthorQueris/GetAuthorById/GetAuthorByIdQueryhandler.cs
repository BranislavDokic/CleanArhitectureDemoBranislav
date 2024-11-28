using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Authors.AuthorQueris.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        public GetAuthorByIdQueryHandler(IGenericRepositoryInterface<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {

            var author = await _authorRepository.GetByIdAsync(request.Id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.Id} was not found.");
            }

            return author;
        }
    }
}
