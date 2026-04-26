using DragonEye.BuildingBlocks.Application.Abstractions;
using DragonEye.Modules.Assets.Application.Abstractions;
using DragonEye.Modules.Assets.Contracts;
using DragonEye.Modules.Assets.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DragonEye.Modules.Assets.Infrastructure.Repositories;

public sealed class AssetRepository(IAppDbContext dbContext) : IAssetRepository
{
    public async Task<IReadOnlyCollection<AssetDto>> GetAllAsync(string? term, CancellationToken cancellationToken)
    {
        var query = dbContext.Assets.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(term)) query = query.Where(x => x.Code.Contains(term) || x.Name.Contains(term) || x.Category.Contains(term));
        return await query.OrderBy(x => x.Code).Select(x => new AssetDto(x.Id, x.Code, x.Name, x.Category, x.IsActive)).ToListAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(CreateAssetRequest request, CancellationToken cancellationToken)
    {
        var entity = new Asset { Code = request.Code, Name = request.Name, Category = request.Category, IsActive = request.IsActive };
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(UpdateAssetRequest request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Assets.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity is null) return false;
        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Category = request.Category;
        entity.IsActive = request.IsActive;
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null) return false;
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
