namespace DragonEye.Modules.Assets.Contracts;

public sealed record AssetDto(Guid Id, string Code, string Name, string Category, bool IsActive);
public sealed record CreateAssetRequest(string Code, string Name, string Category, bool IsActive);
public sealed record UpdateAssetRequest(Guid Id, string Code, string Name, string Category, bool IsActive);
