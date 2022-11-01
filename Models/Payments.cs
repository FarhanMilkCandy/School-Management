using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Payment
    {
        [Key]
        [DisplayName("Payment Id")]
        public int PaymentId { get; set; }

        [Required]
        [DisplayName("Payment Date")]
        public DateOnly PaymentDate { get; set; }

        public virtual Fees Fees { get; set; }
        public virtual Enrollment Enrollments { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
