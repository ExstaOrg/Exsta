using Exsta_Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Data;

namespace UserService.Data;
public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : BaseDbContext(options) {
    public DbSet<User> Users { get; set; }
}
