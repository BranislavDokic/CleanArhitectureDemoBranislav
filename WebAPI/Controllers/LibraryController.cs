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

        public LibraryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<LibraryController>
        [HttpGet]
        public async Task<IActionResult> GetAllLibraries()
        {
            try
            {
                var result = await _mediator.Send(new GetAllLibraryQuery());

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                if (result.Data == null || result.Data.Count == 0)
                {
                    return NotFound("No libraries found.");
                }

                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while fetching libraries.", Details = ex.Message });
            }
        }

        // GET api/<LibraryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLibraryById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetLibraryByIdQuery(id));

                if (result.IsSuccess)
                {
                    return Ok(new { message = result.Message, data = result.Data });
                }

                return NotFound(new { message = result.Message, errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while retrieving the library.", Details = ex.Message });
            }
        }

        // POST api/<LibraryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LibraryDTO newLibrary)
        {
            if (newLibrary == null)
            {
                return BadRequest("Library data is invalid.");
            }

            try
            {
                var result = await _mediator.Send(new CreateLibraryCommand(newLibrary));

                if (result.IsSuccess)
                {
                    return CreatedAtAction(nameof(GetAllLibraries), new { id = result.Data }, new
                    {
                        message = result.Message, 
                        data = result.Data 
                    });
                }

                return BadRequest(new
                {
                    message = result.Message, 
                    errors = result.ErrorMessage 
                });
            }
            catch (Exception ex)
            {
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
            try
            {
                if (updateLibrary == null || string.IsNullOrEmpty(updateLibrary.Name))
                {
                    return BadRequest("Invalid data.");
                }

                var result = await _mediator.Send(new UpdateLibraryCommand(id, updateLibrary.Name));

                if (!result.IsSuccess)
                {
                    if (result.ErrorMessage.Contains("not found"))
                    {
                        return NotFound(new { message = result.Message, errors = result.ErrorMessage });
                    }
                    return StatusCode(500, new { message = result.Message, errors = result.ErrorMessage });
                }

                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while updating the library.", Details = ex.Message });
            }
        }

        // DELETE api/<LibraryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteLibraryCommand(id));

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage, data = result.Data });
                }

                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while deleting the library.", Details = ex.Message });
            }
        }
    }
}
