using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Dtos;
using Application.Books.Queries.GetAllBook;
using Microsoft.AspNetCore.Authorization;
using Application.Books.Queries.GetBookById;
using Application.Authors.AuthorCommands.DeleteAuthor;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly ILogger<BookController> _logger;

        public BookController(IMediator mediatr, ILogger<BookController> logger)
        {
            this._mediatr = mediatr;
            _logger = logger;
        }

        // GET: api/<BookController>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            _logger.LogInformation("Fetching all books at {Time}", DateTime.Now);

            try
            {
                var query = new GetAllBooksQuery();
                var result = await _mediatr.Send(query);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to retrieve books: {ErrorMessage}", result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully retrieved {BookCount} books.", result.Data.Count);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching books.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // GET api/<BookController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            _logger.LogInformation("Fetching book with ID: {BookId} at {Time}", id, DateTime.Now);

            try
            {
                var query = new GetBookByIdQuery(id);
                var result = await _mediatr.Send(query);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to retrieve book with ID {BookId}: {ErrorMessage}", id, result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully retrieved book with ID: {BookId}", id);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching book with ID: {BookId}", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // POST api/<BookController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDTO bookDTO)
        {
            _logger.LogInformation("Start creating book at {Time}", DateTime.Now);

            try
            {
                if (bookDTO == null)
                {
                    _logger.LogWarning("Invalid input: Book data is null.");
                    return BadRequest("Book data is null");
                }

                var createBookCommand = new CreateBookCommand(bookDTO);
                var result = await _mediatr.Send(createBookCommand);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to create book: {ErrorMessage}", result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully created book: {BookTitle}", bookDTO.Title);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the book.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        // PUT api/<BookController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDTO updatedBook)
        {
            _logger.LogInformation("Start updating book with ID: {BookId} at {Time}", id, DateTime.Now);

            try
            {
                if (updatedBook == null)
                {
                    _logger.LogWarning("Invalid input: Book data is null.");
                    return BadRequest("Invalid book data.");
                }

                var result = await _mediatr.Send(new UpdateBookCommand(id, updatedBook));

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to update book with ID {BookId}: {ErrorMessage}", id, result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully updated book with ID: {BookId}", id);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the book.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // DELETE api/<BookController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Attempting to delete book with ID: {BookId} at {Time}", id, DateTime.Now);

            try
            {
                var result = await _mediatr.Send(new DeleteBookCommand(id));

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to delete book with ID {BookId}: {ErrorMessage}", id, result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully deleted book with ID: {BookId}", id);
                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the book.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
