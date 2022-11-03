using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public ICollection<Enrollment>? Enrollments { get; set; }

        public ICollection<Payment>? Payments { get; set; }
    }
}
