using AdvertisementPortal.Models;

namespace AdvertisementPortal.Services
{
    public interface IAdvertisementService
    {
        int Create(CreateAdvertisementDto createDto);
        void Delete(int id);
        void Update(int id, UpdateAdvertisementDto updateDto);
        PagedResoult<AdvertisementDto> GetAll(AdvertisementQuery query);
        AdvertisementDto GetById(int id);
    }
}