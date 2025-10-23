using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Service
{
    public class ServiceItem
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string IconClass { get; set; } = "fa fa-shield-alt";  // e.g. fontawesome icon classes

        [Required, StringLength(100)]
        public string Title { get; set; } = "Cyber Security";

        [Required, StringLength(500)]
        public string Description { get; set; } = "Amet justo dolor lorem kasd amet magna sea stet eos vero lorem ipsum dolore sed";
       
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? LinkUrl { get; set; }  // Optional link for more info or details

        // Foreign key to ServiceSection
        [Required(ErrorMessage = "Please select a service section")]
        public int ServiceSectionId { get; set; }

        public ServiceSection? ServiceSection { get; set; }

    }
}
