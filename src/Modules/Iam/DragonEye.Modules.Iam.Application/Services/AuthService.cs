using DragonEye.Modules.Iam.Application.Abstractions;
using DragonEye.Modules.Iam.Contracts;

namespace DragonEye.Modules.Iam.Application.Services;

public sealed class AuthService(IIamRepository iamRepository)
{
    public Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        => iamRepository.AuthenticateAsync(request, cancellationToken);
}
