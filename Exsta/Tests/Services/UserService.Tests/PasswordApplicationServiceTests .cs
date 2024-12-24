using FluentAssertions;
using System.Text;
using UserService.Application;

namespace UserService.Tests;

public class PasswordApplicationServiceTests {
    private readonly PasswordApplicationService _passwordService;

    public PasswordApplicationServiceTests() {
        var pepper = "ThisIsASecurePepperValue"; // Use a hardcoded pepper for testing purposes
        _passwordService = new PasswordApplicationService(pepper);
    }

    [Fact]
    public void HashPassword_ShouldReturnHashAndSalt() {
        // Arrange
        var password = "SecurePassword123";

        // Act
        var (hashedPassword, salt) = _passwordService.HashPassword(password);

        // Assert
        hashedPassword.Should().NotBeNullOrEmpty();
        salt.Should().NotBeNullOrEmpty();
        hashedPassword.Should().NotBe(password); // Ensure it's not just the raw password
        hashedPassword.Should().NotContain(salt); // Salt should not appear in the hash directly
    }

    [Fact]
    public void HashPassword_ShouldGenerateUniqueSalt() {
        // Arrange
        var password = "SecurePassword123";

        // Act
        var (_, salt1) = _passwordService.HashPassword(password);
        var (_, salt2) = _passwordService.HashPassword(password);

        // Assert
        salt1.Should().NotBe(salt2); // Salt should be unique
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForValidPassword() {
        // Arrange
        var password = "SecurePassword123";
        var (hashedPassword, salt) = _passwordService.HashPassword(password);

        // Act
        var isVerified = _passwordService.VerifyPassword(password, salt, hashedPassword);

        // Assert
        isVerified.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForInvalidPassword() {
        // Arrange
        var password = "SecurePassword123";
        var wrongPassword = "WrongPassword456";
        var (hashedPassword, salt) = _passwordService.HashPassword(password);

        // Act
        var isVerified = _passwordService.VerifyPassword(wrongPassword, salt, hashedPassword);

        // Assert
        isVerified.Should().BeFalse();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForInvalidSalt() {
        // Arrange
        var password = "SecurePassword123";
        var (hashedPassword, salt) = _passwordService.HashPassword(password);
        var wrongSalt = Convert.ToBase64String(Encoding.UTF8.GetBytes("InvalidSalt"));

        // Act
        var isVerified = _passwordService.VerifyPassword(password, wrongSalt, hashedPassword);

        // Assert
        isVerified.Should().BeFalse();
    }
}
