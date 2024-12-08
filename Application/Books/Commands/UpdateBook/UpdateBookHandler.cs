using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;



namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult<UpdateBookDTO>>
    {
        private readonly IGenericRepositoryInterface<Book> _bookRepository;
        private readonly IGenericRepositoryInterface<Genre> _genreRepository;
        private readonly ILogger<UpdateBookCommandHandler> _logger;

        public UpdateBookCommandHandler(IGenericRepositoryInterface<Book> bookRepository, IGenericRepositoryInterface<Genre> genreRepository, ILogger<UpdateBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _logger = logger;
        }
        public async Task<OperationResult<UpdateBookDTO>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update book with ID: {BookId}", request.BookId);

            try
            {
                var bookToUpdate = await _bookRepository.GetByIdAsync(request.BookId);
                if (bookToUpdate == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found.", request.BookId);
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

                _logger.LogInformation("Book with ID {BookId} successfully updated.", request.BookId);
                return OperationResult<UpdateBookDTO>.Success(updatedBookDto, "Book successfully updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the book with ID: {BookId}", request.BookId);
                return OperationResult<UpdateBookDTO>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
