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

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<GenreController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _mediator.Send(new GetAllGenreQuery());
            return Ok(genres);
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            try
            {
                var genre = await _mediator.Send(new GetGenreByIdQuery(id));

                if (genre == null)
                {
                    return NotFound($"Genre with ID {id} not found.");
                }

                return Ok(genre);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> CreateGenre([FromBody] CreateGenreCommand command)
        {
            var genre = await _mediator.Send(command);
            return Ok(genre);
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] string newName)
        {
            try
            {
                var result = await _mediator.Send(new UpdateGenreCommand(id, newName));

                if (result == "Genre updated successfully.")
                {
                    return Ok(result);
                }

                return NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while updating the genre.",
                    Details = ex.Message
                });
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteGenreCommand(id));

                if (result)
                {
                    return Ok($"Genre with ID {id} was successfully deleted.");
                }

                return BadRequest("Failed to delete genre.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while deleting the genre.",
                    Details = ex.Message
                });
            }
        }

    }
}
