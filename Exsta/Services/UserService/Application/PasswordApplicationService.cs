using System.Security.Cryptography;
using System.Text;

namespace UserService.Application;

public class PasswordApplicationService : IPasswordApplicationService {
    private readonly string _pepper; // Loaded securely from configuration

    public PasswordApplicationService(string pepper) {
        _pepper = pepper;
    }

    public (string HashedPassword, string Salt) HashPassword(string password) {
        // Generate a unique salt for this user
        var saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(saltBytes);
        }
        var salt = Convert.ToBase64String(saltBytes);

        // Combine password + salt + pepper
        var combined = password + salt + _pepper;
        var hashedPassword = ComputeHash(combined);

        return (hashedPassword, salt);
    }

    public bool VerifyPassword(string password, string salt, string storedHash) {
        // Recompute the hash with the provided salt and pepper
        var combined = password + salt + _pepper;
        var computedHash = ComputeHash(combined);

        return storedHash == computedHash;
    }

    private string ComputeHash(string input) {
        using var hmac = new HMACSHA256();
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hashBytes);
    }
}
