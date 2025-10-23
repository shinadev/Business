using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Page
{
    public class DynamicPage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Page Title is required")]
        [StringLength(200, ErrorMessage = "Title can't be longer than 200 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "URL Slug is required")]
        [StringLength(150, ErrorMessage = "Slug can't be longer than 150 characters")]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug must be lowercase letters, numbers, and hyphens only")]
        public string Slug { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        public string? Content { get; set; }

        [StringLength(500)]
        public string? MetaDescription { get; set; }

        [StringLength(250)]
        public string? MetaKeywords { get; set; }

        // Foreign Key for PageStatus
        public int PageStatusId { get; set; }

        [ForeignKey("PageStatusId")]
        public virtual PageStatus? Status { get; set; }

        public bool IsPublished { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<PagePlug> PagePlugs { get; set; }
        public virtual ICollection<PageLayoutSection> PageLayoutSections { get; set; }

        public int? PageCategoryId { get; set; }

        [ForeignKey("PageCategoryId")]
        public virtual PageCategory? PageCategory { get; set; }

        public DynamicPage()
        {
            PagePlugs = new HashSet<PagePlug>();
            PageLayoutSections = new HashSet<PageLayoutSection>();
        }
    }
}
