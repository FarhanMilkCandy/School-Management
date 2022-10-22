using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Student
    {
        [DisplayName("Student Id")]
        [Key]
        public int StudentId { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [DisplayName("Gender Id")]
        public int GenderId { get; set; }

        [DisplayName("Religion Id")]
        public int? ReligionId { get; set; }

        [Required]
        [DisplayName("Father's Name")]
        public string FatherName { get; set; }

        [Required]
        [DisplayName("Father's Contact")]
        public string? FatherContact { get; set; }

        [DisplayName("Father's Occupation")]
        public string? FatherOccupation { get; set; }

        [Required]
        [DisplayName("Mother's Name")]
        public string MotherName { get; set; }

        [DisplayName("Mother's Contact")]
        public string? MotherContact { get; set; }

        [DisplayName("Mother's Occupation")]
        public string? MotherOccupation { get; set; }

        [Required]
        [DisplayName("Address")]
        public string? Address { get; set; }

        [DisplayName("Student's Photo")]
        public string? StudentPhoto { get; set; }

        [Required]
        [DisplayName("Status")]
        public bool Status { get; set; }

        [Required]
        [DisplayName("Created By")]
        public string EntryBy { get; set; }

        [Required]
        [DisplayName("Created On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Updated By")]
        public string? UpdatedBy { get; set; }

        [DisplayName("Update On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime? UpdateDate { get; set; }
        
    }
}
