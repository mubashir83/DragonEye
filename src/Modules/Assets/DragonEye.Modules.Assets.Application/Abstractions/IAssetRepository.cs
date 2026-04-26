using DragonEye.Modules.Assets.Contracts;

namespace DragonEye.Modules.Assets.Application.Abstractions;

public interface IAssetRepository
{
    Task<IReadOnlyCollection<AssetDto>> GetAllAsync(string? term, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(CreateAssetRequest request, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(UpdateAssetRequest request, CancellationToken cancellationToken);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
}
