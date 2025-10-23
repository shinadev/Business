using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Blog
{
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string? AuthorImageUrl { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsReply { get; set; }
        public int BlogDetailId { get; set; }
        public BlogDetail? BlogDetail { get; set; }

        [NotMapped]
        public IFormFile? AuthorImageFile { get; set; }
    }
}
