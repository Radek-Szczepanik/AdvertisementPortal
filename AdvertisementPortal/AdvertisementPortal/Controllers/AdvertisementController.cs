using AdvertisementPortal.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AdvertisementPortal.Controllers
{
    [Route("api/advertisement")]
    public class AdvertisementController : ControllerBase
    {
        private readonly AdvertisementDbContext dbContext;

        public AdvertisementController(AdvertisementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Advertisement>> GetAll()
        {
            var advertisement = dbContext.Advertisements.ToList();

            return Ok(advertisement);
        }

        [HttpGet("{id}")]
        public ActionResult<Advertisement> GetById([FromRoute]int id)
        {
            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.Id == id);

            if (advertisement is null)
                return NotFound();

            return Ok(advertisement);
        }
    }
}
