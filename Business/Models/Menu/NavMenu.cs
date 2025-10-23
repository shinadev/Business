using System.ComponentModel.DataAnnotations;

namespace Business.Models.Menu
{
    public class NavMenu
    {
        public int Id { get; set; }

        [Required]
        public string MenuName { get; set; }

        [Required]
        public string Controller { get; set; }

        [Required]
        public string Action { get; set; }

        public string? RouteId { get; set; }

        public string? RouteSlug { get; set; }

        public string? DropdownGroup { get; set; }

        public int Order { get; set; } = 0;

        public bool IsButton { get; set; } = false;
}
    }
