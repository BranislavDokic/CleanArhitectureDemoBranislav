using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Dtos;
using Application.Books.Queries.GetAllBook;
using Microsoft.AspNetCore.Authorization;
using Application.Books.Queries.GetBookById;
using Application.Authors.AuthorCommands.DeleteAuthor;

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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            var result = await _mediatr.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }
            return Ok(new {message = result.Message, data  = result.Data});
        }

        // GET api/<BookController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
           var query = new GetBookByIdQuery(id);
           var result = await _mediatr.Send(query);

           if (!result.IsSuccess)
           {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
           }

           return Ok(new {message = result.Message, data = result.Data});
        }

        // POST api/<BookController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDTO bookDTO)
        {
            if (bookDTO == null)
            {
                return BadRequest("Book data is null");
            }

            var createBookCommand = new CreateBookCommand(bookDTO);

            try
            {
                var result = await _mediatr.Send(createBookCommand);

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                return Ok(new { message = result.Message, data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }


        // PUT api/<BookController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDTO updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest("Invalid book data.");
            }

            var result = await _mediatr.Send(new UpdateBookCommand(id, updatedBook));

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
            }

            return Ok(new { message = result.Message, data = result.Data });
        
        }

        // DELETE api/<BookController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                var result = await _mediatr.Send(new DeleteBookCommand(id));

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Message, errors = result.ErrorMessage });
                }

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
