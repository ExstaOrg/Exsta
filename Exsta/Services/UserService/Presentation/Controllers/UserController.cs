using Exsta_Shared.Domain;
using Exsta_Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application;
using UserService.Repositories;

namespace UserService.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserRepository userRepository, IRegisterUserApplicationService registerUserApplicationService) : ControllerBase {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRegisterUserApplicationService _registerUserApplicationService = registerUserApplicationService;

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
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var result = await _registerUserApplicationService.RegisterUserAsync(registerUserDto);

        if (!result.Success) {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "User registered successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id) {
        await _userRepository.DeleteUserAsync(id);
        return NoContent();
    }
}
