
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Service
{
    public class ServiceHeader
    {

        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; } = "Services";

        [Required, StringLength(250)]
        public string BreadcrumbHomeText { get; set; } = "Home";

        [StringLength(250)]
        public string BreadcrumbHomeUrl { get; set; } = "/";

        [Required, StringLength(250)]
        public string BreadcrumbCurrentText { get; set; } = "Services";

        [StringLength(250)]
        public string BreadcrumbCurrentUrl { get; set; } = "/Services";

        [StringLength(250)]
        public string? ImageUrl { get; set; }  // e.g. "/img/about-header.jpg"

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
