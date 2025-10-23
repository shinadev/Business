using Business.Models.Contact;
using System.Collections.Generic;

namespace Business.ViewModels
{
    public class ContactPageViewModel
    {
        public ContactHeader Header { get; set; } = new ContactHeader();
        public List<ContactSection> Sections { get; set; } = new List<ContactSection>();

        // Optionally, a separate MapUrl if not per-section
        public string MapUrl { get; set; } = "https://www.google.com/maps/embed?pb=!1m18...";
        public string IconClass { get; set; }
    }
}
