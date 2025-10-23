using System.ComponentModel.DataAnnotations;
namespace Business.Models.Home
{
    public class HomeSection
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        public string? ImageUrl { get; set; }  // e.g. "img/carousel-1.jpg"

        [StringLength(150)]
        public string CaptionSmall { get; set; }  // e.g. "Creative & Innovative"

        [StringLength(300)]
        public string CaptionLarge { get; set; }  // e.g. "Creative & Innovative Digital Solution"

        [StringLength(250)]
        public string Button1Text { get; set; }  // e.g. "Free Quote"

        [StringLength(250)]
        public string Button1Url { get; set; }   // e.g. "quote.html"

        [StringLength(250)]
        public string Button2Text { get; set; }  // e.g. "Contact Us"

        [StringLength(250)]
        public string Button2Url { get; set; }   // e.g. "#"

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
