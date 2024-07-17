namespace Athena.Application.Identity.Tokens
{
    public record TokenResponse(
        string UserName,
        string Email,
        IList<string> Roles,
        string Token,
        DateTime TokenExpiryTime,
        string RefreshToken,
        DateTime RefreshTokenExpiryTime
    );
}
