using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Configuration
    {
        [DisplayName("Configuration Id")]
        [Key]
        public int ConfigurationId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string ConfigurationName { get; set; }

        [Required]
        [DisplayName("Short Name")]
        public string ConfigurationShortName { get; set; }

        [Required]
        [DisplayName("Status")]
        public bool Status { get; set; }

        [Required]
        [DisplayName("Created By")]
        public string EntryBy { get; set; }

        [Required]
        [DisplayName("Created On")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Updated By")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public string? UpdatedBy { get; set; }

        [DisplayName("Updated On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime? UpdateDate { get; set; }
    }
}
