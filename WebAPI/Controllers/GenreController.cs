using Application.Dtos;
using Application.Genres.GenresCommands.CreateGenre;
using Application.Genres.GenresCommands.DeleteGenre;
using Application.Genres.GenresCommands.UpdateGenre;
using Application.Genres.GenresQuery.GetAllGenre;
using Application.Genres.GenresQuery.GetGenreById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GenreController> _logger;

        public GenreController(IMediator mediator, ILogger<GenreController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/<GenreController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Attempting to retrieve all genres at {Time}", DateTime.Now);

            try
            {
                var result = await _mediator.Send(new GetAllGenreQuery());

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved {GenreCount} genres.", result.Data.Count);
                    return Ok(new { message = result.Message, data = result.Data });
                }

                _logger.LogWarning("Failed to retrieve genres: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving genres.");
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving genres.", Details = ex.Message });
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            _logger.LogInformation("Attempting to retrieve genre with ID: {GenreId} at {Time}", id, DateTime.Now);

            try
            {
                var result = await _mediator.Send(new GetGenreByIdQuery(id));

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved genre with ID: {GenreId}", id);
                    return Ok(new { message = result.Message, data = result.Data });
                }

                _logger.LogWarning("Failed to retrieve genre with ID {GenreId}: {ErrorMessage}", id, result.ErrorMessage);
                return NotFound(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving genre with ID: {GenreId}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving the genre.", Details = ex.Message });
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> CreateGenre([FromBody] CreateGenreCommand command)
        {
            _logger.LogInformation("Attempting to create a new genre at {Time}", DateTime.Now);

            try
            {
                var result = await _mediator.Send(command);

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully created genre with ID: {GenreId}", result.Data.Id);
                    return Ok(new { message = result.Message, data = result.Data });
                }

                _logger.LogWarning("Failed to create genre: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the genre.");
                return StatusCode(500, new { Message = "An unexpected error occurred while creating the genre.", Details = ex.Message });
            }
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] string newName)
        {
            _logger.LogInformation("Attempting to update genre with ID: {GenreId} at {Time}", id, DateTime.Now);

            try
            {
                var result = await _mediator.Send(new UpdateGenreCommand(id, newName));

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully updated genre with ID: {GenreId}", id);
                    return Ok(new { message = result.Message, data = result.Data });
                }

                _logger.LogWarning("Failed to update genre with ID {GenreId}: {ErrorMessage}", id, result.ErrorMessage);
                return NotFound(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating genre with ID: {GenreId}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred while updating the genre.", Details = ex.Message });
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            _logger.LogInformation("Attempting to delete genre with ID: {GenreId} at {Time}", id, DateTime.Now);

            try
            {
                var result = await _mediator.Send(new DeleteGenreCommand(id));

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully deleted genre with ID: {GenreId}", id);
                    return Ok(new { message = result.Message, data = result.Data });
                }

                _logger.LogWarning("Failed to delete genre with ID {GenreId}: {ErrorMessage}", id, result.ErrorMessage);
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting genre with ID: {GenreId}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred while deleting the genre.", Details = ex.Message });
            }
        }

    }
}
