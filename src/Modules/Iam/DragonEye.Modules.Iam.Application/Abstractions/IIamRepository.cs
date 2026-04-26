using DragonEye.Modules.Iam.Contracts;

namespace DragonEye.Modules.Iam.Application.Abstractions;

public interface IIamRepository
{
    Task<LoginResponse?> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken);
}
