using DragonEye.Modules.Assets.Application.Abstractions;
using DragonEye.Modules.Assets.Contracts;

namespace DragonEye.Modules.Assets.Application.Services;

public sealed class AssetService(IAssetRepository assetRepository)
{
    public Task<IReadOnlyCollection<AssetDto>> GetAllAsync(string? term, CancellationToken cancellationToken)
        => assetRepository.GetAllAsync(term, cancellationToken);

    public Task<Guid> CreateAsync(CreateAssetRequest request, CancellationToken cancellationToken)
        => assetRepository.CreateAsync(request, cancellationToken);

    public Task<bool> UpdateAsync(UpdateAssetRequest request, CancellationToken cancellationToken)
        => assetRepository.UpdateAsync(request, cancellationToken);

    public Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
        => assetRepository.SoftDeleteAsync(id, cancellationToken);
}
