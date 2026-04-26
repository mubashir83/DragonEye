using System.Text.Json;
using DragonEye.BuildingBlocks.Application.Abstractions;
using DragonEye.BuildingBlocks.Domain.Abstractions;
using DragonEye.Modules.Assets.Domain.Entities;
using DragonEye.Modules.Iam.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DragonEye.BuildingBlocks.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService) : DbContext(options), IAppDbContext
{
    public IQueryable<User> Users => Set<User>();
    public IQueryable<Role> Roles => Set<Role>();
    public IQueryable<Permission> Permissions => Set<Permission>();
    public IQueryable<UserRole> UserRoles => Set<UserRole>();
    public IQueryable<RolePermission> RolePermissions => Set<RolePermission>();
    public IQueryable<Asset> Assets => Set<Asset>();
    public IQueryable<AuditLog> AuditLogs => Set<AuditLog>();

    public void Add<TEntity>(TEntity entity) where TEntity : class => Set<TEntity>().Add(entity);
    public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class => Set<TEntity>().AddRange(entities);
    public void Remove<TEntity>(TEntity entity) where TEntity : class => Set<TEntity>().Remove(entity);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var actor = currentUserService.IsAuthenticated ? currentUserService.UserName : "system";
        var entries = ChangeTracker.Entries<AuditableEntity>().Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted).ToList();

        foreach (var entry in entries)
        {
            var oldValues = entry.State == EntityState.Added ? "{}" : JsonSerializer.Serialize(entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p]));

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = actor;
            }
            else
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = actor;
            }

            var newValues = JsonSerializer.Serialize(entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]));
            entry.Entity.RowLog = JsonSerializer.Serialize(new { At = now, By = actor, State = entry.State.ToString(), Values = newValues });

            Add(new AuditLog
            {
                CreatedAt = now,
                CreatedBy = actor,
                Timestamp = now,
                Actor = actor,
                Action = entry.State.ToString(),
                Module = entry.Entity.GetType().Namespace?.Split('.')?.FirstOrDefault(x => x is "Iam" or "Assets") ?? "Core",
                EntityName = entry.Entity.GetType().Name,
                EntityId = entry.Entity.Id.ToString(),
                OldValues = oldValues,
                NewValues = newValues,
                IpAddress = currentUserService.IpAddress,
                CorrelationId = currentUserService.CorrelationId
            });
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
