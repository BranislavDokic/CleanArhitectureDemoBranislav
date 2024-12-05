using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult<UpdateBookDTO>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;
       
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;

        public UpdateBookCommandHandler(IGenericRepositoryInterface<Book> bookRepository, IGenericRepositoryInterface<Genre> genreRepository)
        {
            _bookRepository = bookRepository;
            
            _genreRepository = genreRepository;
        }

        public async Task<OperationResult<UpdateBookDTO>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookToUpdate = await _bookRepository.GetByIdAsync(request.BookId);
                if (bookToUpdate == null)
                {
                    return OperationResult<UpdateBookDTO>.Failure($"Book with ID {request.BookId} not found.");
                }

                if (!string.IsNullOrWhiteSpace(request.UpdatedBook.Title))
                {
                    bookToUpdate.Title = request.UpdatedBook.Title;
                }

                if (!string.IsNullOrWhiteSpace(request.UpdatedBook.Description))
                {
                    bookToUpdate.Description = request.UpdatedBook.Description;
                }

                if (request.UpdatedBook.Genres != null && request.UpdatedBook.Genres.Any())
                {
                    var allGenres = await _genreRepository.GetAllAsync();
                    var matchedGenres = allGenres
                        .Where(g => request.UpdatedBook.Genres.Contains(g.Name, StringComparer.OrdinalIgnoreCase))
                        .ToList();

                    bookToUpdate.Genres = matchedGenres;
                }

                await _bookRepository.UpdateAsync(bookToUpdate.Id, bookToUpdate);

                var updatedBookDto = new UpdateBookDTO
                {
                    Title = bookToUpdate.Title,
                    Description = bookToUpdate.Description,
                    Genres = bookToUpdate.Genres?.Select(g => g.Name).ToList()
                };

                return OperationResult<UpdateBookDTO>.Success(updatedBookDto, "Book successfully updated.");
            }
            catch (Exception ex)
            {
                return OperationResult<UpdateBookDTO>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
