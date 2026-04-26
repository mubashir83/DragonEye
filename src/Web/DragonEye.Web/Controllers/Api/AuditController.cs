using DragonEye.BuildingBlocks.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DragonEye.Web.Controllers.Api;

[ApiController]
[Route("api/audit-logs")]
[Authorize]
public sealed class AuditController(IAppDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "Audit.View")]
    public async Task<IActionResult> Get([FromQuery] string? module, [FromQuery] string? action, [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken cancellationToken = default)
    {
        var query = dbContext.AuditLogs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(module)) query = query.Where(x => x.Module == module);
        if (!string.IsNullOrWhiteSpace(action)) query = query.Where(x => x.Action == action);
        query = query.OrderByDescending(x => x.Timestamp);

        var total = await query.CountAsync(cancellationToken);
        var data = await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
        return Ok(new { total, page, size, data });
    }
}
