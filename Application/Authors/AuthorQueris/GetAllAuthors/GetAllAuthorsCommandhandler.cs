using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Authors.AuthorQueris.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<Author>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;

        public GetAllAuthorsQueryHandler(IGenericRepositoryInterface<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<List<Author>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.GetAllAsync();
            return authors;
        }
    }
}
