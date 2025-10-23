namespace Business.Models.Blog
{
    public class BlogDetail
        {
            public int Id { get; set; }
            public string Title { get; set; }

            public string? MainImageUrl { get; set; } // Nullable
            public List<string> Paragraphs { get; set; } = new List<string>();

            public ICollection<Comment> Comments { get; set; } = new List<Comment>();
            public ICollection<BlogCategory> Categories { get; set; } = new List<BlogCategory>();
            public ICollection<RecentPost> RecentPosts { get; set; } = new List<RecentPost>();

            public string? SidebarImageUrl { get; set; } // Nullable
            public ICollection<Tag> Tags { get; set; } = new List<Tag>();

            public PlainText? PlainText { get; set; } // Nullable
     }

 }