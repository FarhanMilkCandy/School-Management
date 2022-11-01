using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Fees
    {
        [Key]
        [DisplayName("Fee Id")]
        public int FeeId { get; set; }

        [Required]
        [DisplayName("Fee Amount")]
        public double FeeAmount { get; set; }

        [DisplayName("Description")]
        public string? FeeDescription { get; set; }

        public virtual ICollection<Payment>? Payments { get; set; }
    }
}
