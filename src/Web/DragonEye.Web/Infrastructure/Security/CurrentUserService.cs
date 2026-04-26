using System.Security.Claims;
using DragonEye.BuildingBlocks.Application.Abstractions;

namespace DragonEye.Web.Infrastructure.Security;

public sealed class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public string UserId => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    public string UserName => accessor.HttpContext?.User.Identity?.Name ?? "anonymous";
    public string IpAddress => accessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "n/a";
    public string CorrelationId => accessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("N");
    public bool IsAuthenticated => accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public IReadOnlyCollection<string> Permissions => accessor.HttpContext?.User.FindAll("permission").Select(x => x.Value).ToArray() ?? [];
}
