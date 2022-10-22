using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Class
    {
        [DisplayName("Class Id")]
        [Key]
        public int ClassId { get; set; }

        [Required]
        [DisplayName("Class")]
        public string ClassName { get; set; }

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

        [DisplayName("Updated on")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime? UpdateDate { get; set; }

        public List<Section> Sections { get; set; }
    }
}
