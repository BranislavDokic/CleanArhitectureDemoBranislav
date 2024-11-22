using Application.Authors.AuthorCommands.CreateAuthor;
using Application.Authors.AuthorCommands.DeleteAuthor;
using Application.Authors.AuthorCommands.UpdateAuthor;
using Application.Authors.AuthorQueris.GetAllAuthors;
using Application.Authors.AuthorQueris.GetAuthorById;
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
            var authors = await _mediator.Send(query);
            return Ok(authors);
        }

        // GET api/<AuthorController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                var query = new GetAuthorByIdQuery(id);
                var author = await _mediator.Send(query);

                return Ok(author);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST api/<AuthorController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDTO authorToAdd)
        {
            if (authorToAdd == null)
            {
                return BadRequest("Author data is invalid.");
            }

            await _mediator.Send(new CreateAuthorCommand(authorToAdd));

            return Ok();
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
                if (result)
                {
                    return Ok($"Author with ID {id} was successfully updated.");
                }

                return BadRequest("Failed to update author.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                if (result)
                {
                    return Ok($"Author with ID {id} was successfully deleted.");
                }

                return BadRequest("Failed to delete author.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
