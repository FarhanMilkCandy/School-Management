using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace SMS.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
       
        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? Address { get; set; }

        public bool? IsActive { get; set; }
    }
}
