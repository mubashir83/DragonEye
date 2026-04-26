using DragonEye.BuildingBlocks.Domain.Abstractions;
using DragonEye.Modules.Assets.Domain.Entities;
using DragonEye.Modules.Iam.Domain.Entities;

namespace DragonEye.BuildingBlocks.Application.Abstractions;

public interface IAppDbContext
{
    IQueryable<User> Users { get; }
    IQueryable<Role> Roles { get; }
    IQueryable<Permission> Permissions { get; }
    IQueryable<UserRole> UserRoles { get; }
    IQueryable<RolePermission> RolePermissions { get; }
    IQueryable<Asset> Assets { get; }
    IQueryable<AuditLog> AuditLogs { get; }

    void Add<TEntity>(TEntity entity) where TEntity : class;
    void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    void Remove<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
