using AdvertisementPortal.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AdvertisementPortal
{
    public class AdvertisementSeeder
    {
        private readonly AdvertisementDbContext dbContext;

        public AdvertisementSeeder(AdvertisementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            if (dbContext.Database.CanConnect())    // czy jest połączenie z bazą danych ?
            {
                if (!dbContext.Advertisements.Any())    // czy tabela Advertisement jest pusta ?
                {
                    var advertisements = GetAdvertisements();
                    dbContext.Advertisements.AddRange(advertisements);
                    dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Advertisement> GetAdvertisements()
        {
            var advertisements = new List<Advertisement>()
            {
                new Advertisement()
                {
                    Title = "Hydraulik",
                    Content = "usługi hydrauliczne",
                    Email = "hydraulik@test.pl",
                    PhoneNumber = "111-222-333"
                },

                new Advertisement()
                {
                    Title = "Usługi remontowe",
                    Content = "malowanie, gipsowanie," +
                              "układanie płytek",
                    Email = "uslugi_bud@test.pl",
                    PhoneNumber = "444-555-666"
                }
            };

            return advertisements;
        }
    }
}
