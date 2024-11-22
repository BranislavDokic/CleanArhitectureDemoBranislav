using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public BookController(IMediator mediatr)
        {
            this._mediatr = mediatr;
        }



        // GET: api/<BookController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BookController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book bookToAdd)
        {

            if (bookToAdd == null)
            {
                return BadRequest("Book data is invalid.");
            }

            await _mediatr.Send(new CreateBookCommand(bookToAdd));

            return Ok();
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDTO updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest("Invalid book data.");
            }

            try
            {
                var result = await _mediatr.Send(new UpdateBookCommand(id, updatedBook));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediatr.Send(new DeleteBookCommand(id));

            if (!result)
            {
                return NotFound($"No book found with ID {id}");
            }

            return Ok($"Book with ID {id} was successfully deleted.");
        }
    }
}
