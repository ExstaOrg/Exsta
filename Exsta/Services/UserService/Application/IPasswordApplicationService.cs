namespace UserService.Application;

public interface IPasswordApplicationService {
    (string HashedPassword, string Salt) HashPassword(string password);
    bool VerifyPassword(string password, string salt, string storedHash);
}