using Microsoft.AspNetCore.Mvc;
using WebApi.api.Models;
using WebApi.api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApi.api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(UserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET ALL USERS
        [HttpGet(Name = "ReadUsers")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _userRepository.ReadAsync();
            return Ok(users);
        }

        // GET SINGLE USER
        [HttpGet("{username}", Name = "ReadUser")]
        public async Task<ActionResult<User>> Get(string username)
        {
            var user = await _userRepository.ReadAsync(username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<ActionResult<User>> Add(User user)
        {
            _logger.LogInformation("Received request to add user: {@User}", user);

            try
            {
                var existingUser = await _userRepository.ReadAsync(user.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("User with username {Username} already exists.", user.Username);
                    return BadRequest("User with that username already exists.");
                }

                _logger.LogInformation("Inserting new user into database.");
                var createdUser = await _userRepository.InsertAsync(user);

                if (createdUser == null)
                {
                    _logger.LogError("InsertAsync returned null, failed to create user.");
                    return StatusCode(500, "Failed to create user");
                }

                _logger.LogInformation("User created successfully: {Username}", createdUser.Username);
                return CreatedAtAction(nameof(Get), new { username = createdUser.Username }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating user.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        // UPDATE USER (only password can be changed)
        [HttpPut("{username}", Name = "UpdateUser")]
        public async Task<IActionResult> Update(string username, User newUser)
        {
            if (username != newUser.Username)
                return BadRequest("The username in the URL does not match the object");

            var existingUser = await _userRepository.ReadAsync(username);
            if (existingUser == null)
                return NotFound();

            var updatedUser = await _userRepository.UpdateAsync(newUser);
            if (updatedUser == null)
                return StatusCode(500, "Failed to update user");

            return NoContent();
        }

        // DELETE USER
        [HttpDelete("{username}", Name = "DeleteUser")]
        public async Task<IActionResult> Delete(string username)
        {
            var deletedUser = await _userRepository.DeleteAsync(username);
            if (deletedUser == null)
                return NotFound();

            return Ok(deletedUser);
        }
    }
}
