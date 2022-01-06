using AdvertisementPortal.Entities;
using AdvertisementPortal.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace AdvertisementPortal.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly AdvertisementDbContext dbContext;
        private readonly IMapper mapper;

        public AdvertisementService(AdvertisementDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public AdvertisementDto GetById(int id)
        {
            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) return null;

            var advertisementDto = mapper.Map<AdvertisementDto>(advertisement);

            return advertisementDto;
        }

        public IEnumerable<AdvertisementDto> GetAll()
        {
            var advertisement = dbContext.Advertisements.ToList();

            var advertisementDto = mapper.Map<List<AdvertisementDto>>(advertisement);

            return advertisementDto;
        }

        public int Create(CreateAdvertisementDto createDto)
        {
            var advertisement = mapper.Map<Advertisement>(createDto);
            dbContext.Advertisements.Add(advertisement);
            dbContext.SaveChanges();

            return advertisement.Id;
        }
    }
}
