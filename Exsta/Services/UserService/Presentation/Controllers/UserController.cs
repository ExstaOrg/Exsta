using Backend_Shared.Application;
using Exsta_Shared.Domain;
using Exsta_Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application;
using UserService.Repositories;

namespace UserService.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserRepository userRepository,
                            IRegisterUserApplicationService registerUserApplicationService,
                            IAuthService authService) : ControllerBase {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRegisterUserApplicationService _registerUserApplicationService = registerUserApplicationService;
    private readonly IAuthService _authService = authService;

    [AllowAnonymous]
    [HttpOptions]
    public IActionResult Options() {
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] User user) {
        var token = _authService.GenerateToken(user);
        if (token == null) {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(new { Token = token });
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers() {
        var users = await _userRepository.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUserById(int id) {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    [Authorize]
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
    [Authorize]
    public async Task<ActionResult> DeleteUser(int id) {
        await _userRepository.DeleteUserAsync(id);
        return NoContent();
    }
}
