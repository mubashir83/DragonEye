using DragonEye.BuildingBlocks.Application.Abstractions;
using DragonEye.Modules.Iam.Application.Abstractions;
using DragonEye.Modules.Iam.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DragonEye.Modules.Iam.Infrastructure.Repositories;

public sealed class IamRepository(IAppDbContext dbContext, IPasswordHasher passwordHasher) : IIamRepository
{
    public async Task<LoginResponse?> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash)) return null;

        var roleIds = await dbContext.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToListAsync(cancellationToken);
        var permissionIds = await dbContext.RolePermissions.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.PermissionId).ToListAsync(cancellationToken);
        var permissions = await dbContext.Permissions.Where(x => permissionIds.Contains(x.Id)).Select(x => x.Name).Distinct().ToListAsync(cancellationToken);

        return new LoginResponse(user.Id, user.FullName, user.Email, permissions);
    }
}
