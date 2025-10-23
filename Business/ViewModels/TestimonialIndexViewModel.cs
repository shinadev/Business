using Business.Models.Testimonial;
using Business.Models.Vendor;

namespace Business.ViewModels
{
    public class TestimonialIndexViewModel
    {
        public TestimonialSection Section { get; set; }
        public IEnumerable<Testimonials> Testimonials { get; set; }
    }
}
