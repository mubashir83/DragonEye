namespace DragonEye.BuildingBlocks.Application.Abstractions;

public interface ICurrentUserService
{
    string UserId { get; }
    string UserName { get; }
    string IpAddress { get; }
    string CorrelationId { get; }
    bool IsAuthenticated { get; }
    IReadOnlyCollection<string> Permissions { get; }
}
