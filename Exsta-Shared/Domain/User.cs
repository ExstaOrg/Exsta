namespace Exsta_Shared.Domain;

public class User {
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }

    public required string[] Roles { get; set; }
}