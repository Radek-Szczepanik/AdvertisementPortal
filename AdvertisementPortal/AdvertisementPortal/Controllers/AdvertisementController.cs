using AdvertisementPortal.Entities;
using AdvertisementPortal.Models;
using AdvertisementPortal.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AdvertisementPortal.Controllers
{
    [Route("api/advertisement")]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService advertisementService;

        public AdvertisementController(IAdvertisementService advertisementService)
        {
            this.advertisementService = advertisementService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AdvertisementDto>> GetAll()
        {
            var advertisementDtos = advertisementService.GetAll();

            return Ok(advertisementDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<AdvertisementDto> Get([FromRoute]int id)
        {
            var advertisement = advertisementService.GetById(id);

            if (advertisement == null)
                return NotFound();

            return Ok(advertisement);
        }

        [HttpPost]
        public ActionResult CreateAdvertisement([FromBody] CreateAdvertisementDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = advertisementService.Create(createDto);

            return Created($"/api/advertisement/{id}", null);
        }
    }
}
