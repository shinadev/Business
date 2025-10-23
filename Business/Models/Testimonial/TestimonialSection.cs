namespace Business.Models.Testimonial
{
    public class TestimonialSection
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; } = "Testimonial";
        public string MainHeading { get; set; } = "What Our Clients Say About Our Digital Services";
        public List<Testimonials> Testimonials { get; set; } = new List<Testimonials>();
    }
}
