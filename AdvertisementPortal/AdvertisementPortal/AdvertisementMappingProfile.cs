using AdvertisementPortal.Entities;
using AdvertisementPortal.Models;
using AutoMapper;

namespace AdvertisementPortal
{
    public class AdvertisementMappingProfile : Profile
    {
        public AdvertisementMappingProfile()
        {
            CreateMap<Advertisement, AdvertisementDto>();

            CreateMap<CreateAdvertisementDto, Advertisement>();
        }
    }
}
