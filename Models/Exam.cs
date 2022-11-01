using Microsoft.Build.Framework;
using System.ComponentModel;

namespace SMS.Models
{
    public class Exam
    {
        [DisplayName("Exam Id")]
        public string ExamId { get; set; }
        
        [Required]
        [DisplayName("Exam Type")]
        public int ExamType { get; set; }

        [Required]
        [DisplayName("Fee Id")]
        public string FeeId { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        List<Fees> Fees { get; set; }
    }
}
