using System.Security.Claims;

namespace Athena.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        string? Name { get; }

        Guid GetUserId();
        void SetUserId(string userId);

        Guid GetBusinessId();
        void SetBusinessId(string businessId); // New setter method for businessId

        string? GetUserEmail();

        bool IsAuthenticated();

        bool IsInRole(string role);

        IEnumerable<Claim>? GetUserClaims();
    }
}
