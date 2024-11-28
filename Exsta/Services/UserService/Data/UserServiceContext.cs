using Exsta_Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Data;

namespace UserService.Data;
public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : BaseDbContext(options) {
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>(entity => {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Roles)
                  .HasConversion(
                      v => string.Join(',', v),    // Serialize roles to a single string
                      v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)) // Deserialize back to array
                  .IsRequired();
        });
    }
}
