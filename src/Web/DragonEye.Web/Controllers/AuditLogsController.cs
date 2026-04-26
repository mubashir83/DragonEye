using DragonEye.BuildingBlocks.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DragonEye.Web.Controllers;

[Authorize(Policy = "Audit.View")]
public sealed class AuditLogsController(IAppDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index(string? module, string? action, int page = 1, int size = 20, CancellationToken cancellationToken = default)
    {
        var query = dbContext.AuditLogs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(module)) query = query.Where(x => x.Module == module);
        if (!string.IsNullOrWhiteSpace(action)) query = query.Where(x => x.Action == action);
        query = query.OrderByDescending(x => x.Timestamp);
        ViewData["Total"] = await query.CountAsync(cancellationToken);
        return View(await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken));
    }
}
