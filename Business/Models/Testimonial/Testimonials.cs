using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Testimonial
{
    public class Testimonials
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string Quote { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }  // nullable for safety

        // Foreign key
        public int? TestimonialSectionId { get; set; }

        [ForeignKey("TestimonialSectionId")]
        public TestimonialSection? TestimonialSection { get; set; }
    }
}
