using System.ComponentModel.DataAnnotations;

namespace Business.Models.Menu
{
    public class FooterMenu
    {
        public int Id { get; set; }

        public string? AboutTitle { get; set; }
        public string? AboutContent { get; set; }

        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? Copyright { get; set; }
        public string? AttributionText { get; set; }
        public string? AttributionUrl { get; set; }

        public ICollection<FooterLink>? QuickLinks { get; set; }
        public ICollection<FooterLink>? PopularLinks { get; set; }
        public ICollection<FooterSocial>? Socials { get; set; }
    }

}
