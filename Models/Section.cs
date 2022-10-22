using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Section
    {
        [DisplayName("Admission Id")]
        [Key]
        public int SectionId { get; set; }

        [Required]
        [DisplayName("Section Name")]
        public string SectionName { get; set; }

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
        
        [DisplayName("Updated On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime? UpdateDate { get; set; }
        
    }
}
