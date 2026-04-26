using DragonEye.BuildingBlocks.Application.Abstractions;
using DragonEye.BuildingBlocks.Infrastructure.Persistence;
using DragonEye.Modules.Assets.Application.Abstractions;
using DragonEye.Modules.Assets.Application.Services;
using DragonEye.Modules.Assets.Infrastructure.Repositories;
using DragonEye.Modules.Iam.Application.Abstractions;
using DragonEye.Modules.Iam.Application.Services;
using DragonEye.Modules.Iam.Infrastructure.Repositories;
using DragonEye.Web.Infrastructure.Data;
using DragonEye.Web.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AssetService>();
builder.Services.AddScoped<IIamRepository, IamRepository>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => { options.LoginPath = "/Account/Login"; options.AccessDeniedPath = "/Account/Login"; });
builder.Services.AddAuthorization(options => { options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); options.AddPolicy("Hardware.View", p => p.RequireClaim("permission", "Hardware.View")); options.AddPolicy("Hardware.Create", p => p.RequireClaim("permission", "Hardware.Create")); options.AddPolicy("Hardware.Edit", p => p.RequireClaim("permission", "Hardware.Edit")); options.AddPolicy("Hardware.Delete", p => p.RequireClaim("permission", "Hardware.Delete")); options.AddPolicy("Audit.View", p => p.RequireClaim("permission", "Audit.View")); });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}
await SeedData.EnsureSeededAsync(app);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
