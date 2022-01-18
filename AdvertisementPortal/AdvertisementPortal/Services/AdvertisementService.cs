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
        private readonly IUserContextService userContextService;

        public AdvertisementService(AdvertisementDbContext dbContext, IMapper mapper, ILogger<AdvertisementService> logger,
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
        }

        public AdvertisementDto GetById(int id)
        {
            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) throw new NotFoundException("Advertisement not found");

            var advertisementDto = mapper.Map<AdvertisementDto>(advertisement);

            return advertisementDto;
        }

        public IEnumerable<AdvertisementDto> GetAll(string searchPhrase)
        {
            var advertisement = dbContext.Advertisements.Where(r => searchPhrase == null || (r.Title.ToLower().Contains(searchPhrase.ToLower())
                                                            || r.Content.ToLower().Contains(searchPhrase.ToLower()))).ToList();

            var advertisementDto = mapper.Map<List<AdvertisementDto>>(advertisement);

            return advertisementDto;
        }

        public int Create(CreateAdvertisementDto createDto)
        {
            var advertisement = mapper.Map<Advertisement>(createDto);
            advertisement.CreatedById = userContextService.GetUserId;
            dbContext.Advertisements.Add(advertisement);
            dbContext.SaveChanges();

            return advertisement.Id;
        }

        public void Delete(int id)
        {
            logger.LogError($"Advertisement with id: {id} delete");

            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) throw new NotFoundException("Advertisement not found");

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, advertisement,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Advertisements.Remove(advertisement);
            dbContext.SaveChanges();
        }

        public void Update(int id, UpdateAdvertisementDto updateDto)
        {
            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null) throw new NotFoundException("Advertisement not found");

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, advertisement,
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
