using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Application.Users.UserQueris.GetAllUsers;
using Domain;
using Application.Dtos;
using Application.Users.UserCommand;
using Application.Users.UserQueris.UserLogin;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public UsersController(IMediator mediatr)
        {
            this._mediatr = mediatr;
        }

        // GET: api/<UsersController>
        [HttpGet]
        [Route ("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _mediatr.Send(new GetAllUsersQueri());

                if (result.IsSuccess)
                {
                    return Ok(new { Message = result.Message, Users = result.Data });
                }

                return BadRequest(new { Message = result.Message, Errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userToAdd)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userToAdd?.UserName) || string.IsNullOrWhiteSpace(userToAdd?.Password))
                {
                    return BadRequest("UserName and Password cannot be empty or whitespace.");
                }

                var result = await _mediatr.Send(new AddNewUserCommand(userToAdd));

                if (result.IsSuccess)
                {
                    return Ok(new { Message = result.Message, User = result.Data });
                }

                return BadRequest(new { Message = result.Message, Errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userWantingToLogin)
        {
            try
            {
                var result = await _mediatr.Send(new UserLoginQueri(userWantingToLogin));

                if (result.IsSuccess)
                {
                    return Ok(new { Message = result.Message, Token = result.Data });
                }

                return BadRequest(new { Message = result.Message, Errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
