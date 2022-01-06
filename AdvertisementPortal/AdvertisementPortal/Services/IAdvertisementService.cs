using AdvertisementPortal.Models;
using System.Collections.Generic;

namespace AdvertisementPortal.Services
{
    public interface IAdvertisementService
    {
        int Create(CreateAdvertisementDto createDto);
        IEnumerable<AdvertisementDto> GetAll();
        AdvertisementDto GetById(int id);
    }
}