using System.ComponentModel.DataAnnotations;

namespace Business.Models.Page
{
    public class PageStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }  
    }
}
