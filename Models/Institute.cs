using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Institute
    {
        [Required]
        [Key]
        public int EIIN { get; set; }

        [Required]
        [DisplayName("Institute Name")]
        public string InstituteName { get; set; }

        [Required]
        [DisplayName("Established On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateOnly EstablishedDate { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Contact No.")]
        public string Contact { get; set; }

        [Required]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required]
        [DisplayName("Created By")]
        public string EntryBy { get; set; }

        [Required]
        [DisplayName("Created On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Updated By")]
        public string UpdatedBy { get; set; }


        [DisplayName("Update On")]
        [DisplayFormat(ApplyFormatInEditMode = true , DataFormatString ="{0:dd/MM/yyyy hh/mm/ss}")]
        public DateTime UpdateDate { get; set; }


    }
}
