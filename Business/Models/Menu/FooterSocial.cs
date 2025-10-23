using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Menu
{
    public class FooterSocial
    {
        public int Id { get; set; }

        public string Platform { get; set; } // e.g. Twitter
        public string IconClass { get; set; } // e.g. fab fa-twitter
        public string Url { get; set; }

        [ForeignKey("FooterSection")]
        public int FooterMenuId { get; set; }

        public FooterMenu FooterMenu { get; set; }
    }

}
