using DragonEye.BuildingBlocks.Domain.Abstractions;

namespace DragonEye.Modules.Iam.Domain.Entities;

public sealed class RolePermission : AuditableEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}
