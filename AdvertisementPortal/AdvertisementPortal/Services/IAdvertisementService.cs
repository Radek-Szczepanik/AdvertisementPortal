using AdvertisementPortal.Models;
using System.Collections.Generic;

namespace AdvertisementPortal.Services
{
    public interface IAdvertisementService
    {
        int Create(CreateAdvertisementDto createDto);
        void Delete(int id);
        void Update(int id, UpdateAdvertisementDto updateDto);
        IEnumerable<AdvertisementDto> GetAll(string searchPhrase);
        AdvertisementDto GetById(int id);
    }
}