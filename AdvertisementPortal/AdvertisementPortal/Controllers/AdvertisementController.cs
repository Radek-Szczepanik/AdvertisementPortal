using AdvertisementPortal.Entities;
using AdvertisementPortal.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AdvertisementPortal.Controllers
{
    [Route("api/advertisement")]
    public class AdvertisementController : ControllerBase
    {
        private readonly AdvertisementDbContext dbContext;
        private readonly IMapper mapper;

        public AdvertisementController(AdvertisementDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AdvertisementDto>> GetAll()
        {
            var advertisement = dbContext.Advertisements.ToList();

            var advertisementDtos = mapper.Map<List<AdvertisementDto>>(advertisement);

            return Ok(advertisementDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<AdvertisementDto> GetById([FromRoute]int id)
        {
            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null)
                return NotFound();

            var advertisementDto = mapper.Map<AdvertisementDto>(advertisement);

            return Ok(advertisementDto);
        }
    }
}
