using System.ComponentModel.DataAnnotations;

namespace Business.Models.Page
{
    public class LayoutSection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // e.g., "Hero Section", "FAQ", "Footer", etc.

        [StringLength(250)]
        public string Description { get; set; }
    }
}
