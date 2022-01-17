using AdvertisementPortal.Authorization;
using AdvertisementPortal.Entities;
using AdvertisementPortal.Exceptions;
using AdvertisementPortal.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AdvertisementPortal.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly AdvertisementDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<AdvertisementService> logger;
        private readonly IAuthorizationService authorizationService;

        public AdvertisementService(AdvertisementDbContext dbContext, IMapper mapper, ILogger<AdvertisementService> logger,
            IAuthorizationService authorizationService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
        }

        public AdvertisementDto GetById(int id)
        {
            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) throw new NotFoundException("Advertisement not found");

            var advertisementDto = mapper.Map<AdvertisementDto>(advertisement);

            return advertisementDto;
        }

        public IEnumerable<AdvertisementDto> GetAll()
        {
            var advertisement = dbContext.Advertisements.ToList();

            var advertisementDto = mapper.Map<List<AdvertisementDto>>(advertisement);

            return advertisementDto;
        }

        public int Create(CreateAdvertisementDto createDto, int userId)
        {
            var advertisement = mapper.Map<Advertisement>(createDto);
            advertisement.CreatedById = userId;
            dbContext.Advertisements.Add(advertisement);
            dbContext.SaveChanges();

            return advertisement.Id;
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            logger.LogError($"Advertisement with id: {id} delete");

            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) throw new NotFoundException("Advertisement not found");

            var authorizationResult = authorizationService.AuthorizeAsync(user, advertisement,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Advertisements.Remove(advertisement);
            dbContext.SaveChanges();
        }

        public void Update(int id, UpdateAdvertisementDto updateDto, ClaimsPrincipal user)
        {
            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) throw new NotFoundException("Advertisement not found");

            var authorizationResult = authorizationService.AuthorizeAsync(user, advertisement,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            advertisement.Title = updateDto.Title;
            advertisement.Content = updateDto.Content;

            dbContext.SaveChanges();
        }
    }
}
