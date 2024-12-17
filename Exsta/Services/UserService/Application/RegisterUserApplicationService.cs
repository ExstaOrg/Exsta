using Exsta_Shared.Domain;
using Exsta_Shared.DTO;
using UserService.Repositories;

namespace UserService.Application;

public class RegisterUserApplicationService(IUserRepository userRepository) : IRegisterUserApplicationService {

    private readonly IUserRepository _userRepository = userRepository;

    public async Task<(bool Success, string[]? Errors)> RegisterUserAsync(RegisterUserDto registerUserDto) {
        // Check if the user already exists
        var existingUser = await _userRepository.GetUserByEmailAsync(registerUserDto.Email);
        if (existingUser != null) {
            return (false, new string[] { "User already exists." });
        }

        // Hash the password
        var hashedPassword = "";//TODO: _passwordHasher.HashPassword(registerUserDto.Password);

        // Create the user entity
        var newUser = new User {
            Username = "",
            Email = registerUserDto.Email,
            PasswordHash = hashedPassword,
            Roles = registerUserDto.Roles ?? ["User"]
        };

        // Save the user to the repository
        await _userRepository.AddUserAsync(newUser);

        return (true, null);
    }
}