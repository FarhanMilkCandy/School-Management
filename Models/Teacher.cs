using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Teacher
    {
        [DisplayName("Teacher Id")]
        [Key]
        public int TeacherId { get; set; }

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
        [DisplayName("Designation Id")]
        public int DesignationId { get; set; }

        [DisplayName("Gender Id")]
        public int? GenderId { get; set; }

        [DisplayName("Religion Id")]
        public int? ReligionId { get; set; }

        [Required]
        [DisplayName("Contact No.")]
        public string Contact { get; set; }

        [DisplayName("Email")]
        public string? Email { get; set; }

        [Required]
        [DisplayName("Email")]
        public string Address { get; set; }

        [DisplayName("Teacher's Photo")]
        public string? TeacherPhoto { get; set; }

        [Required]
        [DisplayName("Joining Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateOnly JoiningDate { get; set; }

        [DisplayName("Leaving Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateOnly? LeavingDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public bool Status { get; set; }

        [Required]
        [DisplayName("Created By")]
        public string EntryBy { get; set; }

        [Required]
        [DisplayName("Created On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateOnly EntryDate { get; set; }

        [DisplayName("Updated By")]
        public string? UpdatedBy { get; set; }

        [DisplayName("Updated On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateOnly? UpdateDate { get; set; }

    }
}
