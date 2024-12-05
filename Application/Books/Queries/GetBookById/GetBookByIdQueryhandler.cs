using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Books.Queries.GetBookById
{
    public class GetBookByIdQueryhandler : IRequestHandler<GetBookByIdQuery, OperationResult<BookDTO>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;

        public GetBookByIdQueryhandler(IGenericRepositoryInterface<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<BookDTO>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
           
            var book = await _bookRepository.GetByIdAsync(request.Id, query => query
                .Include(b => b.Author)   
                .Include(b => b.Library) 
                .Include(b => b.Genres)   
            );

            

            if (book != null)
            {
                var bookDTO = new BookDTO(book);

                return OperationResult<BookDTO>.Success(bookDTO, $"Successfully returned book by Id {request.Id}");
            }

            return OperationResult<BookDTO>.Failure($"Failure to return book by Id {request.Id}", "Operation Failed");
        }

    }
}
