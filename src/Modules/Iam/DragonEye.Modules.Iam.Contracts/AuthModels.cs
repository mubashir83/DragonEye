namespace DragonEye.Modules.Iam.Contracts;

public sealed record LoginRequest(string Email, string Password);
public sealed record LoginResponse(Guid UserId, string FullName, string Email, IReadOnlyCollection<string> Permissions);
