using System.ComponentModel.DataAnnotations;

namespace AdvertisementPortal.Models
{
    public class CreateAdvertisementDto
    {
        [Required, MaxLength(30)]
        public string Title { get; set; }
        [Required, MaxLength(1000)]        
        public string Content { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}