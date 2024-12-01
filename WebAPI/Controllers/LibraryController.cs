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
                var libraries = await _mediator.Send(new GetAllLibraryQuery());

                if (libraries == null || libraries.Count == 0)
                {
                    return NotFound("No libraries found.");
                }

                return Ok(libraries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<LibraryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLibraryById(int id)
        {
            try
            {
                var library = await _mediator.Send(new GetLibraryByIdQuery(id));

                if (library == null)
                {
                    return NotFound($"Library with ID {id} not found.");
                }

                return Ok(library); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                return CreatedAtAction(nameof(GetAllLibraries), result); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

                if (result == "Library updated successfully")
                {
                    return Ok($"Library with ID {id} has been updated.");
                }
                else if (result == "Library not found")
                {
                    return NotFound($"Library with ID {id} not found.");
                }
                else
                {
                    return StatusCode(500, "Internal server error.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/<LibraryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteLibraryCommand(id));

                if (result == "Deleted")
                {
                    return Ok($"Library with ID {id} has been deleted.");
                }
                else if (result == "Library not found")
                {
                    return NotFound($"Library with ID {id} not found.");
                }
                else
                {
                    return StatusCode(500, $"Internal server error: {result}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
