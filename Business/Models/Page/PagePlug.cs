using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models.Page
{
    public class PagePlug
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(150)]
        public string Slug { get; set; }  // no [Required]

        [StringLength(500)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public int DynamicPageId { get; set; }

        [ForeignKey("DynamicPageId")]
        public DynamicPage? Page { get; set; }  // make nullable
    }
}
