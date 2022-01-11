using AdvertisementPortal.Entities;
using AdvertisementPortal.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace AdvertisementPortal.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly AdvertisementDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<AdvertisementService> logger;

        public AdvertisementService(AdvertisementDbContext dbContext, IMapper mapper, ILogger<AdvertisementService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
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

        public bool Delete(int id)
        {
            logger.LogError($"Advertisement with id: {id} delete");

            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) return false;

            dbContext.Advertisements.Remove(advertisement);
            dbContext.SaveChanges();

            return true;
        }

        public bool Update(int id, UpdateAdvertisementDto updateDto)
        {
            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) return false;

            advertisement.Title = updateDto.Title;
            advertisement.Content = updateDto.Content;

            dbContext.SaveChanges();

            return true;
        }
    }
}
