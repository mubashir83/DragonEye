using DragonEye.BuildingBlocks.Domain.Abstractions;

namespace DragonEye.Modules.Iam.Domain.Entities;

public sealed class UserRole : AuditableEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
