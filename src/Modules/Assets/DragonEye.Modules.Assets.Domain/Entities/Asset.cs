using DragonEye.BuildingBlocks.Domain.Abstractions;

namespace DragonEye.Modules.Assets.Domain.Entities;

public sealed class Asset : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
