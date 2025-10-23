using System.ComponentModel.DataAnnotations;

namespace Business.Models.Contact
{
    public class ContactSection
    {
        public int Id { get; set; }

        [Required]
        public string SmallTitle { get; set; } = "Contact Us";

        [Required]
        public string MainTitle { get; set; } = "If You Have Any Query, Feel Free To Contact Us";

        // Contact Info
        [Required]
        public string PhoneTitle { get; set; } = "Call to ask any question";

        [Required]
        public string PhoneNumber { get; set; } = "+012 345 6789";

        [Required]
        public string EmailTitle { get; set; } = "Email to get free quote";

        [Required]
        public string EmailAddress { get; set; } = "info@example.com";

        [Required]
        public string OfficeTitle { get; set; } = "Visit our office";

        [Required]
        public string OfficeAddress { get; set; } = "123 Street, NY, USA";

        // Form placeholders
        [Required]
        public string NamePlaceholder { get; set; } = "Your Name";

        [Required]
        public string EmailPlaceholder { get; set; } = "Your Email";

        [Required]
        public string SubjectPlaceholder { get; set; } = "Subject";

        [Required]
        public string MessagePlaceholder { get; set; } = "Message";

        // Button Text
        [Required]
        public string ButtonText { get; set; } = "Send Message";

        // Google Map URL
        [Required]
        public string MapUrl { get; set; } = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3001156.4288297426!2d-78.01371936852176!3d42.72876761954724!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x4ccc4bf0f123a5a9%3A0xddcfc6c1de189567!2sNew%20York%2C%20USA!5e0!3m2!1sen!2sbd!4v1603794290143!5m2!1sen!2sbd";
    }
}
