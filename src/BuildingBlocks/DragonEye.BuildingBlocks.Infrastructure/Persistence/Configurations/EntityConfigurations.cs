using DragonEye.BuildingBlocks.Domain.Abstractions;
using DragonEye.Modules.Assets.Domain.Entities;
using DragonEye.Modules.Iam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragonEye.BuildingBlocks.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("IAM_Users");
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(x => x.Email).HasMaxLength(320);
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("IAM_Roles");
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("IAM_Permissions");
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("IAM_UserRoles");
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();
    }
}
public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("IAM_RolePermissions");
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique();
    }
}
public sealed class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("AZS_Assets");
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(x => x.Name).HasMaxLength(200);
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AUD_Logs");
        builder.Property(x => x.Actor).HasMaxLength(256);
        builder.Property(x => x.IpAddress).HasMaxLength(128);
        builder.Property(x => x.CorrelationId).HasMaxLength(128);
        builder.Property(x => x.Module).HasMaxLength(64);
        builder.Property(x => x.Action).HasMaxLength(32);
    }
}
