using Application.Dtos;
using Application.Library.LibraryCommand.CreateLibrary;
using Application.Library.LibraryCommand.DeleteLibrary;
using Application.Library.LibraryCommand.UpdateLibrary;
using Application.Library.LibraryQuery.GetAllLibrary;
using Application.Library.LibraryQuery.GetLibraryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(IMediator mediator, ILogger<LibraryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/<LibraryController>
        [HttpGet]
        public async Task<IActionResult> GetAllLibraries()
        {
            _logger.LogInformation("Fetching all libraries at {Time}", DateTime.Now);

            try
            {
                var result = await _mediator.Send(new GetAllLibraryQuery());

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to retrieve libraries: {ErrorMessage}", result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                if (result.Data == null || result.Data.Count == 0)
                {
                    _logger.LogInformation("No libraries found.");
                    return NotFound("No libraries found.");
                }

                _logger.LogInformation("Successfully retrieved {LibraryCount} libraries.", result.Data.Count);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching libraries.");
                return StatusCode(500, new { Message = "An unexpected error occurred while fetching libraries.", Details = ex.Message });
            }
        }

        // GET api/<LibraryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLibraryById(int id)
        {
            _logger.LogInformation("Fetching library with ID: {LibraryId} at {Time}", id, DateTime.Now);

            try
            {
                var result = await _mediator.Send(new GetLibraryByIdQuery(id));

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved library with ID: {LibraryId}", id);
                    return Ok(new { message = result.Message, data = result.Data });
                }

                _logger.LogWarning("Library with ID: {LibraryId} not found.", id);
                return NotFound(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving the library with ID: {LibraryId}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving the library.", Details = ex.Message });
            }
        }

        // POST api/<LibraryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LibraryDTO newLibrary)
        {
            _logger.LogInformation("Start creating library at {Time}", DateTime.Now);

            if (newLibrary == null)
            {
                _logger.LogWarning("Invalid input: Library data is null.");
                return BadRequest("Library data is invalid.");
            }

            try
            {
                var result = await _mediator.Send(new CreateLibraryCommand(newLibrary));

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully created library with ID: {LibraryId}", result.Data);
                    return CreatedAtAction(nameof(GetAllLibraries), new { id = result.Data }, new
                    {
                        message = result.Message,
                        data = result.Data
                    });
                }

                _logger.LogWarning("Failed to create library: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(new
                {
                    message = result.Message,
                    errors = result.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the library.");
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while creating the library.",
                    Details = ex.Message
                });
            }
        }

        // PUT api/<LibraryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LibraryDTO updateLibrary)
        {
            _logger.LogInformation("Start updating library with ID: {LibraryId} at {Time}", id, DateTime.Now);

            try
            {
                if (updateLibrary == null || string.IsNullOrEmpty(updateLibrary.Name))
                {
                    _logger.LogWarning("Invalid input: Library data is null or missing name.");
                    return BadRequest("Invalid data.");
                }

                var result = await _mediator.Send(new UpdateLibraryCommand(id, updateLibrary.Name));

                if (!result.IsSuccess)
                {
                    if (result.ErrorMessage.Contains("not found"))
                    {
                        _logger.LogWarning("Library with ID: {LibraryId} not found for update.", id);
                        return NotFound(new { message = result.Message, errors = result.ErrorMessage });
                    }

                    _logger.LogError("Failed to update library with ID: {LibraryId}. Error: {ErrorMessage}", id, result.ErrorMessage);
                    return StatusCode(500, new { message = result.Message, errors = result.ErrorMessage });
                }

                _logger.LogInformation("Successfully updated library with ID: {LibraryId}", id);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the library with ID: {LibraryId}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred while updating the library.", Details = ex.Message });
            }
        }

        // DELETE api/<LibraryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Attempting to delete library with ID: {LibraryId} at {Time}", id, DateTime.Now);

            try
            {
                var result = await _mediator.Send(new DeleteLibraryCommand(id));

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to delete library with ID: {LibraryId}. Error: {ErrorMessage}", id, result.ErrorMessage);
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage, data = result.Data });
                }

                _logger.LogInformation("Successfully deleted library with ID: {LibraryId}", id);
                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the library with ID: {LibraryId}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred while deleting the library.", Details = ex.Message });
            }
        }
    }
}
