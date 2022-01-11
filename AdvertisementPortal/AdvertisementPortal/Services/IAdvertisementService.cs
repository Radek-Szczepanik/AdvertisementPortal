using AdvertisementPortal.Models;
using System.Collections.Generic;

namespace AdvertisementPortal.Services
{
    public interface IAdvertisementService
    {
        int Create(CreateAdvertisementDto createDto);
        bool Delete(int id);
        bool Update(int id, UpdateAdvertisementDto updateDto);
        IEnumerable<AdvertisementDto> GetAll();
        AdvertisementDto GetById(int id);
    }
}