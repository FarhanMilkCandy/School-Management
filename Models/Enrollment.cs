using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using SMS.Enums;

namespace SMS.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        public string EnrollmentName { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public virtual Course Course { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Payment>? Payments { get; set; }

    }
}
