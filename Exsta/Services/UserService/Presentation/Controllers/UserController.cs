using Exsta_Shared.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Repositories;

namespace UserService.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserRepository userRepository) : ControllerBase {
    private readonly IUserRepository _userRepository = userRepository;

    [HttpOptions]
    [AllowAnonymous]
    public IActionResult Options() {
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers() {
        var users = await _userRepository.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id) {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> AddUser(User user) {
        await _userRepository.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, User user) {
        if (id != user.Id) {
            return BadRequest();
        }

        await _userRepository.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id) {
        await _userRepository.DeleteUserAsync(id);
        return NoContent();
    }
}
