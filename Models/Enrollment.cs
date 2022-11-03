using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using SMS.Enums;

namespace SMS.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        public DateOnly EnrollmentDate { get; set; }

        [Required]
        public int CoursesId { get; set; }

        [Required]
        [ForeignKey("Id")]
        public string StudentId { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public ApplicationUser? Student { get; set; }

        public Course? Courses { get; set; }

        public ICollection<Payment>? Payments { get; set; }
    }
}
