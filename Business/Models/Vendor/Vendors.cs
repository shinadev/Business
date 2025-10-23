using System.ComponentModel.DataAnnotations;

namespace Business.Models.Vendor
{
    public class Vendors
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string? ImagePath { get; set; } // e.g., "img/vendor-1.jpg"

        [Required]
        [StringLength(255)]
        public string AltText { get; set; } // e.g., "Vendor 1"

        [StringLength(255)]
        public string Link { get; set; } // Optional vendor website link
    }
}
