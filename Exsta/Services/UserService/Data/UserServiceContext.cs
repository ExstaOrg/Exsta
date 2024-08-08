using Microsoft.EntityFrameworkCore;
using Shared.Data;
using UserService.Models;

namespace UserService.Data;
public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : BaseDbContext(options) {
    public DbSet<User> Users { get; set; }
}
