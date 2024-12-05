using Application.Authors.AuthorCommands.CreateAuthor;
using Application.Authors.AuthorCommands.DeleteAuthor;
using Application.Authors.AuthorCommands.UpdateAuthor;
using Application.Authors.AuthorQueris.GetAllAuthors;
using Application.Authors.AuthorQueris.GetAuthorById;
using Application.Books.Commands.DeleteBook;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediatr)
        {
            _mediator = mediatr;
        }


        // GET: api/<AuthorController>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var query = new GetAllAuthorsQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        // GET api/<AuthorController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var query = new GetAuthorByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }

            return Ok(new { message = result.Message, data = result.Data });
        }

        // POST api/<AuthorController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDTO authorToAdd)
        {
            if (string.IsNullOrWhiteSpace(authorToAdd?.Name) || string.IsNullOrWhiteSpace(authorToAdd?.Biography))
            {
                return BadRequest("Name and Biography cannot be empty or whitespace.");
            }

            try
            {
                var result = await _mediator.Send(new CreateAuthorCommand(authorToAdd));

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        // PUT api/<AuthorController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorDTO updatedAuthor)
        {
            if (updatedAuthor == null)
            {
                return BadRequest("Invalid author data.");
            }

            try
            {
                var result = await _mediator.Send(new UpdateAuthorCommand(id, updatedAuthor.Name, updatedAuthor.Biography));

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                return Ok(new { message = result.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // DELETE api/<AuthorController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteAuthorCommand(id));

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage, data = result.Data });
                }

                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while deleting the author.", Details = ex.Message });
            }
        }
    }
}
