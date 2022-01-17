using AdvertisementPortal.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace AdvertisementPortal.Services
{
    public interface IAdvertisementService
    {
        int Create(CreateAdvertisementDto createDto, int userId);
        void Delete(int id, ClaimsPrincipal user);
        void Update(int id, UpdateAdvertisementDto updateDto, ClaimsPrincipal user);
        IEnumerable<AdvertisementDto> GetAll();
        AdvertisementDto GetById(int id);
    }
}