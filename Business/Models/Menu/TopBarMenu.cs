
namespace Business.Models.Menu
{
    public class TopBarMenu
    {
        public int Id { get; set; }

        // Contact Info
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Social Media Links
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string YouTubeUrl { get; set; }
    }
}