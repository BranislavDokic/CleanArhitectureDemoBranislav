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
using System.Text.Json;
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
            var books = await _mediatr.Send(query); 
            return Ok(books); 
        }

        // GET api/<BookController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var query = new GetBookByIdQuery(id);
                var book = await _mediatr.Send(query);
                return Ok(book);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST api/<BookController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDTO bookToAdd)
        {
            if (bookToAdd.AuthorId <= 0)
            {
                return BadRequest("Invalid AuthorId");
            }

            try
            {
                var createdBooks = await _mediatr.Send(new CreateBookCommand(bookToAdd));

                var newBook = createdBooks.LastOrDefault();

                if (newBook == null)
                {
                    return StatusCode(500, "Book could not be added.");
                }

                return Ok(new { Message = "Book has been successfully added.", Book = newBook });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }


        // PUT api/<BookController>/5
        [Authorize]
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
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                var result = await _mediatr.Send(new DeleteBookCommand(id));

                if (result)
                {
                    return Ok($"Book with ID {id} was successfully deleted.");
                }

                return NotFound($"Book with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
