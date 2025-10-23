using Business.Models.Testimonial;
using Business.Models.Vendor;
using System.Collections.Generic;

namespace Business.ViewModels
{
    public class TestimonialPageViewModel
    {
        public TestimonialSection TestimonialSection { get; set; } = new TestimonialSection();
        public List<Vendors> Vendors { get; set; } = new List<Vendors>();
    }
}
