using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Service
{
    public class ServiceSection
    {
        [Key]
        public int Id { get; set; }
        // Section Titles
        [Required, StringLength(100)]
        public string SmallTitle { get; set; } = "Our Services";

        [Required, StringLength(200)]
        public string MainTitle { get; set; } = "Custom IT Solutions for Your Successful Business";

        // List of individual services
        public List<ServiceItem>? Services { get; set; } = new List<ServiceItem>();

        // Optional call to action block
        public CallToAction? CallUsForQuote { get; set; }
    }
}
