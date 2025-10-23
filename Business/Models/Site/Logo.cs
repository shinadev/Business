using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Business.Models.Site
{
    public class Logo
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = "Website Logo";

        [StringLength(250)]
        public string? ImageUrl { get; set; }  // e.g. "/images/logo.png"

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
