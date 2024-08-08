using Microsoft.EntityFrameworkCore;

namespace Shared.Data;
public interface IDbContext {
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public class BaseDbContext : DbContext, IDbContext {
    public BaseDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);
    }
}
