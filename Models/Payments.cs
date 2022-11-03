using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        public int EnrollmentsId { get; set; }

        public Enrollment? Enrollments { get; set; }

        [Required]
        public int FeesId { get; set; }

        public Fees? Fees { get; set; }

        [Required]
        [DisplayName("Paid By")]
        [ForeignKey("Id")]
        public string PayeeId { get; set; }

        public ApplicationUser? Payee { get; set; }
    }
}
