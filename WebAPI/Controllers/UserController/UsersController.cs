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
            return Ok(await _mediatr.Send(new GetAllUsersQueri()));
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
            return Ok(await _mediatr.Send(new AddNewUserCommand(userToAdd)));
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userWantingToLogin)
        {
            return Ok(await _mediatr.Send(new UserLoginQueri(userWantingToLogin)));
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
