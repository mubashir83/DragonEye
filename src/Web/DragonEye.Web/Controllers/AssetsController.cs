using DragonEye.Modules.Assets.Application.Services;
using DragonEye.Modules.Assets.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DragonEye.Web.Controllers;

[Authorize]
public sealed class AssetsController(AssetService assetService) : Controller
{
    [Authorize(Policy = "Hardware.View")]
    public async Task<IActionResult> Index(string? term, CancellationToken cancellationToken)
        => View(await assetService.GetAllAsync(term, cancellationToken));

    [Authorize(Policy = "Hardware.Create")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateAssetRequest request, CancellationToken cancellationToken)
    {
        await assetService.CreateAsync(request, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "Hardware.Delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await assetService.SoftDeleteAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
