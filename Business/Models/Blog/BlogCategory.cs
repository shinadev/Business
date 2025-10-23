namespace Business.Models.Blog
{
    public class BlogCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int BlogDetailId { get; set; }
        public BlogDetail? BlogDetail { get; set; }
    }
}
