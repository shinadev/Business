using System.ComponentModel.DataAnnotations;

namespace Business.Models.Menu
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name = "Slug (optional)")]
        public string? Slug { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        public int Order { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
