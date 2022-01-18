using System.Security.Claims;

namespace AdvertisementPortal.Services
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}