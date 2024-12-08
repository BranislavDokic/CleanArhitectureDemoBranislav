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
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IMediator mediatr, ILogger<AuthorController> logger)
        {
            _mediator = mediatr;
            _logger = logger;
        }


        // GET: api/<AuthorController>
        [Authorize]
        [ResponseCache (CacheProfileName = "DefaultCache")]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            _logger.LogInformation("Fetching all authors at {Time}", DateTime.Now);

            try
            {
                var query = new GetAllAuthorsQuery();
                var result = await _mediator.Send(query);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to retrieve authors: {ErrorMessage}", result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully retrieved {AuthorCount} authors.", result.Data.Count);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching authors.");
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // GET api/<AuthorController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            _logger.LogInformation("Fetching author with ID: {AuthorId} at {Time}", id, DateTime.Now);

            if (id <= 0)
            {
                ModelState.AddModelError("Id", "Författarens ID måste vara större än 0.");
                return BadRequest(ModelState); 
            }

            try
            { 
                var query = new GetAuthorByIdQuery(id);
                var result = await _mediator.Send(query);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to retrieve author with ID {AuthorId}: {ErrorMessage}", id, result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully retrieved author with ID: {AuthorId}", id);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching author with ID: {AuthorId}", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        // POST api/<AuthorController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDTO authorToAdd)
        {
            _logger.LogInformation("Start creating author at {Time}", DateTime.Now);
            try
            {
                if (string.IsNullOrWhiteSpace(authorToAdd?.Name) || string.IsNullOrWhiteSpace(authorToAdd?.Biography))
                {
                    _logger.LogWarning("Invalid input: Author name or biography is empty or whitespace.");
                    return BadRequest("Name and Biography cannot be empty or whitespace.");
                }

                _logger.LogInformation("Creating author: {AuthorName}", authorToAdd.Name);
                var result = await _mediator.Send(new CreateAuthorCommand(authorToAdd));

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to create author {AuthorName}: {ErrorMessage}", authorToAdd.Name, result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully created author: {AuthorName}", authorToAdd.Name);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating author: {AuthorName}", authorToAdd.Name);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        // PUT api/<AuthorController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorDTO updatedAuthor)
        {
            _logger.LogInformation("Start updating author with ID: {AuthorId} at {Time}", id, DateTime.Now);
            try
            {
                if (updatedAuthor == null)
                {
                    _logger.LogWarning("Invalid input: Author data is null.");
                    return BadRequest("Invalid author data.");
                }

                _logger.LogInformation("Updating author with ID: {AuthorId}", id);
                var result = await _mediator.Send(new UpdateAuthorCommand(id, updatedAuthor.Name, updatedAuthor.Biography));

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to update author with ID {AuthorId}: {ErrorMessage}", id, result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully updated author with ID: {AuthorId}", id);
                return Ok(new { message = result.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found: {ErrorMessage}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating author with ID: {AuthorId}", id);
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // DELETE api/<AuthorController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Attempting to delete author with ID: {AuthorId} at {Time}", id, DateTime.Now);
            try
            {
                _logger.LogInformation("Attempting to delete author with ID: {AuthorId}", id);
                var result = await _mediator.Send(new DeleteAuthorCommand(id));

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to delete author with ID {AuthorId}: {ErrorMessage}", id, result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage, data = result.Data });
                }

                _logger.LogInformation("Successfully deleted author with ID: {AuthorId}", id);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the author with ID: {AuthorId}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred while deleting the author.", Details = ex.Message });
            }
        }
    }
}
