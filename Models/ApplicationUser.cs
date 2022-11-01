using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace SMS.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string? Address { get; set; }

        [DisplayName("Active Status")]
        public bool? IsActive { get; set; }

        public virtual ICollection<Enrollment>? Enrollments { get; set; }
    }
}
