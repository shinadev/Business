namespace Business.Models.Team
{
    public class TeamSection
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; } = "Team Members";
        public string MainHeading { get; set; } = " Professional Stuffs Ready to Help Your Business";
        public List<TeamMember> TeamMember { get; set; } = new List<TeamMember>();
    }
}
