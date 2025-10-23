using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Business.Models.Page
{
    public class PageLayoutSection
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Page is required")]
        public int DynamicPageId { get; set; }

        [ForeignKey("DynamicPageId")]
        [ValidateNever]
        public DynamicPage? Page { get; set; }

        [Required(ErrorMessage = "Layout Section is required")]
        public int LayoutSectionId { get; set; }

        [ForeignKey("LayoutSectionId")]
        [ValidateNever]
        public LayoutSection? LayoutSection { get; set; }

        [StringLength(250)]
        public string? Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Content { get; set; }

        public int SortOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
