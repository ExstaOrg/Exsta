using UserService.Models;

namespace UserService.Repositories;
public interface IUserRepository {
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync( int id );
    Task AddUserAsync( User user );
    Task UpdateUserAsync( User user );
    Task DeleteUserAsync( int id );
}
