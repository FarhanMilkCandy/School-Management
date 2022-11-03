using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models
{
    public class Fees
    {
        [Key]
        public int FeeId { get; set; }

        [Required]
        [DisplayName("Fee Amount")]
        public double FeeAmount { get; set; }

        [DisplayName("Description")]
        public string? FeeDescription { get; set; }

        [Required]
        public int CoursesId { get; set; }

        public Course? Courses { get; set; }

        public ICollection<Payment>? Payments { get; set; }
    }
}
