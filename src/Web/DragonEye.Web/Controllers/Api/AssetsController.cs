using DragonEye.Modules.Assets.Application.Services;
using DragonEye.Modules.Assets.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DragonEye.Web.Controllers.Api;

[ApiController]
[Route("api/assets")]
[Authorize]
public sealed class AssetsController(AssetService assetService) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "Hardware.View")]
    public async Task<IReadOnlyCollection<AssetDto>> GetAll([FromQuery] string? term, CancellationToken cancellationToken)
        => await assetService.GetAllAsync(term, cancellationToken);

    [HttpPost]
    [Authorize(Policy = "Hardware.Create")]
    public async Task<IActionResult> Create([FromBody] CreateAssetRequest request, CancellationToken cancellationToken)
        => Ok(await assetService.CreateAsync(request, cancellationToken));

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Hardware.Edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssetRequest request, CancellationToken cancellationToken)
        => await assetService.UpdateAsync(request with { Id = id }, cancellationToken) ? Ok() : NotFound();

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Hardware.Delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => await assetService.SoftDeleteAsync(id, cancellationToken) ? Ok() : NotFound();
}
