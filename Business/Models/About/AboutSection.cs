  using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace Business.Models.About
    {
        public class AboutSection
        {
            [Key]
            public int Id { get; set; }

            // Section titles
            [Required, StringLength(100)]
            public string Subtitle { get; set; } = "About Us";

            [Required, StringLength(200)]
            public string Title { get; set; } = "The Best IT Solution With 10 Years of Experience";

            // Main description
            [Required, StringLength(1000)]
            public string Description { get; set; }

            // Features (you can store them as separate fields or a list if needed)
            [StringLength(100)]
            public string Feature1 { get; set; } = "Award Winning";

            [StringLength(100)]
            public string Feature2 { get; set; } = "Professional Staff";

            [StringLength(100)]
            public string Feature3 { get; set; } = "24/7 Support";

            [StringLength(100)]
            public string Feature4 { get; set; } = "Fair Prices";

            // Contact info
            [StringLength(200)]
            public string ContactText { get; set; } = "Call to ask any question";

            [StringLength(50)]
            public string PhoneNumber { get; set; } = "+012 345 6789";

            [StringLength(200)]
            public string QuoteButtonText { get; set; } = "Request A Quote";

            [StringLength(250)]
            public string QuoteButtonUrl { get; set; } = "quote.html";

            // Image
            [StringLength(250)]
            public string? ImageUrl { get; set; } // e.g. "/img/about.jpg"

            [NotMapped]
            public IFormFile? ImageFile { get; set; }
        }
    }

