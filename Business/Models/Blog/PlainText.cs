namespace Business.Models.Blog
{
    public class PlainText
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string ReadMoreLink { get; set; }
        public int BlogDetailId { get; set; }
        public BlogDetail? BlogDetail { get; set; }
    }
}
