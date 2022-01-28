using AdvertisementPortal.Models;

namespace AdvertisementPortal.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
}
