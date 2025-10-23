using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using Microsoft.AspNetCore.Http;

namespace Business.Models.Team
{
    public class TeamMember
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(100)]
        public string Designation { get; set; }

        // Path to the uploaded image (stored in DB)
        [StringLength(255)]
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // Social Media Links
        [StringLength(255)]
        public string? TwitterUrl { get; set; }

        [StringLength(255)]
        public string? FacebookUrl { get; set; }

        [StringLength(255)]
        public string? InstagramUrl { get; set; }

        [StringLength(255)]
        public string? LinkedInUrl { get; set; }

        public int? TeamSectionId { get; set; }

        [ForeignKey("TeamSectionId")]
        public TeamSection? TeamSection { get; set; }
    }
}
