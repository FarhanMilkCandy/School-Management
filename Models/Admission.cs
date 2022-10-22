using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models
{
    public class Admission
    {
        [DisplayName("Admission Id")]
        [Key]
        public int AdmissionId { get; set; }

        [Required]
        [DisplayName("Registration Id")]   
        public int RegistrationId { get; set; }

        [Required]

        [DisplayName("Student Id")]
        public int StudentId { get; set; }

        [DisplayName("Roll No.")]
        public int StudentRoll { get; set; }

        [Required]
        [DisplayName("Admission Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateOnly AdmissionDate { get; set; }

        [Required]
        [DisplayName("Class Id")]
        public int ClassId { get; set; }

        [Required]
        [DisplayName("Session")]
        public string Session { get; set; }

        [Required]
        [DisplayName("Created By")]
        public string EntryBy { get; set; }

        [Required]
        [DisplayName("Created On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Updated By")]
        public string? UpdatedBy { get; set; }

        [DisplayName("Updated On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime? UpdateDate { get; set; }
    }
}
