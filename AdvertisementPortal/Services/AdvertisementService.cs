using AdvertisementPortal.Authorization;
using AdvertisementPortal.Entities;
using AdvertisementPortal.Exceptions;
using AdvertisementPortal.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

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

        public PagedResoult<AdvertisementDto> GetAll(AdvertisementQuery query)
        {
            var baseQuery = dbContext
                .Advertisements
                .Where(r => query.SearchPhrase == null || (r.Title.ToLower().Contains(query.SearchPhrase.ToLower())
                                                            || r.Content.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Advertisement, object>>>
                {
                    { nameof(Advertisement.Title), r => r.Title },
                    { nameof(Advertisement.Content), r => r.Content },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var advertisement = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var advertisementDto = mapper.Map<List<AdvertisementDto>>(advertisement);

            var result = new PagedResoult<AdvertisementDto>(advertisementDto, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
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
