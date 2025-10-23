using System.ComponentModel.DataAnnotations;

namespace Business.Models.User
{
   
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property
        public ICollection<AdminUser>? AdminUsers { get; set; }
    }

}
