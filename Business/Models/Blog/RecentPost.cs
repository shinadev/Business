using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Blog
{
    public class RecentPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string Link { get; set; }
        public int BlogDetailId { get; set; }
        public BlogDetail? BlogDetail { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
