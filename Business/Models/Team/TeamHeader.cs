using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Team
{
    public class TeamHeader
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; } = "Team Members";

        [Required, StringLength(250)]
        public string BreadcrumbHomeText { get; set; } = "Home";

        [StringLength(250)]
        public string BreadcrumbHomeUrl { get; set; } = "/";

        [Required, StringLength(250)]
        public string BreadcrumbCurrentText { get; set; } = "Team Members";

        [StringLength(250)]
        public string BreadcrumbCurrentUrl { get; set; } = "/team";

        [StringLength(250)]
        public string? ImageUrl { get; set; }  // e.g. "/img/about-header.jpg"

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
