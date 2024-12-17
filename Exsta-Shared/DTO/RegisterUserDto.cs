namespace Exsta_Shared.DTO;

public class RegisterUserDto {
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string[]? Roles { get; set; }
}