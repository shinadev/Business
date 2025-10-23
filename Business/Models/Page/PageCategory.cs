using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Page
{
    public class PageCategory
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "Category Name can't be longer than 100 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        // Navigation property
        public ICollection<DynamicPage> Pages { get; set; } = new List<DynamicPage>();
    }
}
