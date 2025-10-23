
using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.User
{
    public class LoginHistory
    {
        public int Id { get; set; }

        [Required]
        public string AdminEmail { get; set; }

        public string? AdminUsername { get; set; }

        public string? Role { get; set; }

        public DateTime Timestamp { get; set; }

        public string Action { get; set; }

        public string? IPAddress { get; set; }
    }
}

