using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Business.Models.Site
{
    public class Favicon
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = "Website Favicon";

        [Required, StringLength(150)]
        public string Title { get; set; } = "My Website";  // Title shown in the browser

        [StringLength(250)]
        public string? ImageUrl { get; set; }  // e.g. "/images/favicon.ico"

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
