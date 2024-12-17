using Exsta_Shared.Domain;
using Microsoft.EntityFrameworkCore;
using UserService.Data;

namespace UserService.Repositories;
public class UserRepository : IUserRepository {
    private readonly UserServiceDbContext _context;

    public UserRepository(UserServiceDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync() {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id) {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByEmailAsync(string email) {
        return await _context.Users.Where(user => string.Equals(email, user.Email, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefaultAsync();
    }

    public async Task AddUserAsync(User user) {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user) {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id) {
        var user = await _context.Users.FindAsync(id);
        if (user != null) {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
