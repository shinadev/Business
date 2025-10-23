using Business.Models.Blog;

namespace Business.ViewModels
{
    public class BlogDetailViewModel
    {
        public BlogDetail Blog { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<BlogCategory> Categories { get; set; }
        public IEnumerable<RecentPost> RecentPosts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public PlainText PlainText { get; set; }
        public BlogHeader BlogHeader { get; set; }
    }
}
