using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.About
{
    public class AboutHeader
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; } = "About Us";

        [Required, StringLength(250)]
        public string BreadcrumbHomeText { get; set; } = "Home";

        [StringLength(250)]
        public string BreadcrumbHomeUrl { get; set; } = "/";

        [Required, StringLength(250)]
        public string BreadcrumbCurrentText { get; set; } = "About";

        [StringLength(250)]
        public string BreadcrumbCurrentUrl { get; set; } = "/about";

        [StringLength(250)]
        public string? ImageUrl { get; set; }  // e.g. "/img/about-header.jpg"

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
