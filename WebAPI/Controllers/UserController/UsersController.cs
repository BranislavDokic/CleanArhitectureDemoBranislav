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
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediatr, ILogger<UsersController> logger)
        {
            _mediatr = mediatr;
            _logger = logger;
        }

        // GET: api/<UsersController>
        [HttpGet]
        [Route ("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users at {Time}", DateTime.Now);

            try
            {
                var result = await _mediatr.Send(new GetAllUsersQueri());

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully retrieved {UserCount} users.", result.Data.Count);
                    return Ok(new { Message = result.Message, Users = result.Data });
                }

                _logger.LogWarning("Failed to retrieve users: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(new { Message = result.Message, Errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching users.");
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
            _logger.LogInformation("Attempting to register new user at {Time}", DateTime.Now);

            try
            {
                if (string.IsNullOrWhiteSpace(userToAdd?.UserName) || string.IsNullOrWhiteSpace(userToAdd?.Password))
                {
                    _logger.LogWarning("Invalid input: UserName or Password is empty.");
                    return BadRequest("UserName and Password cannot be empty or whitespace.");
                }

                var result = await _mediatr.Send(new AddNewUserCommand(userToAdd));

                if (result.IsSuccess)
                {
                    _logger.LogInformation("Successfully registered new user with UserName: {UserName}", userToAdd.UserName);
                    return Ok(new { Message = result.Message, User = result.Data });
                }

                _logger.LogWarning("Failed to register user: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(new { Message = result.Message, Errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while registering the user.");
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userWantingToLogin)
        {
            _logger.LogInformation("Attempting to log in user at {Time}", DateTime.Now);

            try
            {
                var result = await _mediatr.Send(new UserLoginQueri(userWantingToLogin));

                if (result.IsSuccess)
                {
                    _logger.LogInformation("User logged in successfully with UserName: {UserName}", userWantingToLogin.UserName);
                    return Ok(new { Message = result.Message, Token = result.Data });
                }

                _logger.LogWarning("Failed to log in user: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(new { Message = result.Message, Errors = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while logging in the user.");
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
