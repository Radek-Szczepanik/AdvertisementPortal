using System.ComponentModel.DataAnnotations;

namespace AdvertisementPortal.Entities
{
    public class Advertisement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
