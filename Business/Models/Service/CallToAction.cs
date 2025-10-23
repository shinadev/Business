using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Service
{
    public class CallToAction
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = "Call Us For Quote";

        [Required, StringLength(300)]
        public string Description { get; set; } = "Clita ipsum magna kasd rebum at ipsum amet dolor justo dolor est magna stet eirmod";

        [Required, StringLength(20)]
        public string PhoneNumber { get; set; } = "+012 345 6789";

        // Foreign key to ServiceSection (optional)
        public int? ServiceSectionId { get; set; }

        [ForeignKey(nameof(ServiceSectionId))]
        public ServiceSection? ServiceSection { get; set; }
    }
}
