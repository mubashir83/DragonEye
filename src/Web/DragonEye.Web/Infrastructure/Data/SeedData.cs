using DragonEye.BuildingBlocks.Application.Abstractions;
using DragonEye.Modules.Assets.Domain.Entities;
using DragonEye.Modules.Iam.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DragonEye.Web.Infrastructure.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        if (await db.Users.AnyAsync()) return;

        var adminRole = new Role { Name = "Admin" };
        var userRole = new Role { Name = "User" };
        db.AddRange([adminRole, userRole]);

        var permissionNames = new[] { "Hardware.View", "Hardware.Create", "Hardware.Edit", "Hardware.Delete", "Audit.View" };
        var permissions = permissionNames.Select(x => new Permission { Name = x }).ToList();
        db.AddRange(permissions);

        var adminUser = new User { FullName = "System Administrator", Email = "admin@dragoneye.local", PasswordHash = hasher.Hash("Admin@123") };
        db.Add(adminUser);
        await db.SaveChangesAsync();

        db.Add(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
        db.AddRange(permissions.Select(p => new RolePermission { RoleId = adminRole.Id, PermissionId = p.Id }));

        db.AddRange([
            new Asset { Code = "HW-001", Name = "Edge Gateway", Category = "Gateway", IsActive = true },
            new Asset { Code = "HW-002", Name = "Temperature Sensor", Category = "Sensor", IsActive = true }
        ]);

        await db.SaveChangesAsync();
    }
}
