using DragonEye.BuildingBlocks.Domain.Abstractions;

namespace DragonEye.Modules.Iam.Domain.Entities;

public sealed class User : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
