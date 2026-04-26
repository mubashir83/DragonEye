using DragonEye.BuildingBlocks.Domain.Abstractions;

namespace DragonEye.Modules.Iam.Domain.Entities;

public sealed class Role : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
}
