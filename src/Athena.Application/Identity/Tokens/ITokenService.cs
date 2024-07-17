namespace Athena.Application.Identity.Tokens
{
    public interface ITokenService : ITransientService
    {
        Task<TokenResponse> GetTokenAsync(TokenRequest request, string role, string ipAddress, CancellationToken cancellationToken);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string role, string ipAddress);
    }
}
