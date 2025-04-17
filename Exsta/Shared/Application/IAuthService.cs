using Exsta_Shared.Domain;

namespace Backend_Shared.Application;
public interface IAuthService {
    string GenerateToken(User user);
}