namespace UserService.Application;

public interface IRegisterUserApplicationService {
    public Task<(bool Success, string[]? Errors)> RegisterUserAsync(RegisterUserDto registerUserDto);
}