using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Menu
{
    public class FooterLink
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Url { get; set; } = "#";

        public FooterLinkType LinkType { get; set; } // Quick or Popular

        [ForeignKey("FooterMenu")]
        public int FooterMenuId { get; set; }

        public FooterMenu FooterMenu { get; set; }
    }

    public enum FooterLinkType
    {
        Quick,
        Popular
    }
 }
