using AdvertisementPortal.Entities;
using AdvertisementPortal.Models;
using AdvertisementPortal.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AdvertisementPortal.Controllers
{
    [Route("api/advertisement")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService advertisementService;

        public AdvertisementController(IAdvertisementService advertisementService)
        {
            this.advertisementService = advertisementService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<AdvertisementDto>> GetAll([FromQuery] string searchPhrase)
        {
            var advertisementDtos = advertisementService.GetAll(searchPhrase);

            return Ok(advertisementDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<AdvertisementDto> Get([FromRoute]int id)
        {
            var advertisement = advertisementService.GetById(id);

            return Ok(advertisement);
        }

        [HttpPost]
        public ActionResult CreateAdvertisement([FromBody] CreateAdvertisementDto createDto)
        {
            var userId = advertisementService.Create(createDto);
            
            return Created($"/api/advertisement/{userId}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateAdvertisementDto updateDto)
        {
            advertisementService.Update(id, updateDto);
            
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            advertisementService.Delete(id);

            return NoContent();
        }
    }
}
