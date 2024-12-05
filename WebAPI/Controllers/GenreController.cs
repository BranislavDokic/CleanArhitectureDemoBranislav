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
            try
            {
                var result = await _mediator.Send(new GetAllGenreQuery());

                if (result.IsSuccess)
                {
                    return Ok(new { message = result.Message, data = result.Data });
                }

                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving genres.", Details = ex.Message });
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetGenreByIdQuery(id));

                if (result.IsSuccess)
                {
                    return Ok(new { message = result.Message, data = result.Data });
                }

                return NotFound(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving the genre.", Details = ex.Message });
            }
        }

        // POST api/<GenreController>
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> CreateGenre([FromBody] CreateGenreCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] string newName)
        {
            try
            {
                var result = await _mediator.Send(new UpdateGenreCommand(id, newName));

                if (result.IsSuccess)
                {
                    return Ok(new { message = result.Message, data = result.Data });
                }

                return NotFound(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while updating the genre.", Details = ex.Message });
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteGenreCommand(id));

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage, data = result.Data });
                }

                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while deleting the genre.", Details = ex.Message });
            }

        }

    }
}
