using Exsta_Shared.Domain;
using Exsta_Shared.DTO;
using FluentAssertions;
using Moq;
using UserService.Application;
using UserService.Repositories;

public class RegisterUserApplicationServiceTests {
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPasswordApplicationService> _passwordServiceMock = new();
    private readonly RegisterUserApplicationService _service;

    public RegisterUserApplicationServiceTests() {
        _service = new RegisterUserApplicationService(_userRepositoryMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnError_WhenUserAlreadyExists() {
        // Arrange
        var existingUser = new User {
            Email = "test@example.com",
            Username = "",
            PasswordHash = "",
            PasswordSalt = "",
            Roles = []
        };

        _userRepositoryMock
            .Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(existingUser);

        var registerUserDto = new RegisterUserDto {
            Email = "test@example.com",
            Password = "",
            Username = ""
        };

        // Act
        var (Success, Errors) = await _service.RegisterUserAsync(registerUserDto);

        // Assert
        Success.Should().BeFalse();
        Errors.Should().ContainSingle("User already exists.");
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldRegisterUser_WhenUserDoesNotExist() {
        // Arrange
        _userRepositoryMock
            .Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _passwordServiceMock
            .Setup(service => service.HashPassword(It.IsAny<string>()))
            .Returns(("hashedPassword", "salt"));

        var registerUserDto = new RegisterUserDto {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123",
            Roles = ["User"]
        };

        // Act
        var (Success, Errors) = await _service.RegisterUserAsync(registerUserDto);

        // Assert
        Success.Should().BeTrue();
        Errors.Should().BeNull();

        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(user =>
            user.Email == "test@example.com" &&
            user.PasswordHash == "hashedPassword" &&
            user.PasswordSalt == "salt" &&
            user.Username == "testuser" &&
            user.Roles.Contains("User")
        )), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldHandleRepositoryError() {
        // Arrange
        _userRepositoryMock
            .Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database error"));

        _userRepositoryMock
            .Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _passwordServiceMock
            .Setup(service => service.HashPassword(It.IsAny<string>()))
            .Returns(("hashedPassword", "salt"));

        var registerUserDto = new RegisterUserDto {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123"
        };

        // Act
        Func<Task> act = async () => await _service.RegisterUserAsync(registerUserDto);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");

        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
    }
}
