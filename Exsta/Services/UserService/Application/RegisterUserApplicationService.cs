using Exsta_Shared.Domain;
using Exsta_Shared.DTO;
using UserService.Repositories;

namespace UserService.Application;

public class RegisterUserApplicationService(IUserRepository userRepository, IPasswordApplicationService passwordApplicationService) : IRegisterUserApplicationService {

    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordApplicationService _passwordApplicationService = passwordApplicationService;

    public async Task<(bool Success, string[]? Errors)> RegisterUserAsync(RegisterUserDto registerUserDto) {
        // Check if the user already exists
        var existingUser = await _userRepository.GetUserByEmailAsync(registerUserDto.Email);
        if (existingUser != null) {
            return (false, new string[] { "User already exists." });
        }

        // Hash the password
        (var hashedPassword, var salt) = _passwordApplicationService.HashPassword(registerUserDto.Password);

        // Create the user entity
        var newUser = new User {
            Username = registerUserDto.Username,
            Email = registerUserDto.Email,
            PasswordHash = hashedPassword,
            PasswordSalt = salt,
            Roles = registerUserDto.Roles ?? ["User"]
        };

        // Save the user to the repository
        await _userRepository.AddUserAsync(newUser);

        return (true, null);
    }
}