using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NBE_Project.Models
{
    public class NDA
    {
        public int Id { get; set; }

        [StringLength(255, ErrorMessage = "File name cannot exceed 255 characters.")]
        public string? FileName { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "File content is required.")]
        public IFormFile? NDAFile { get; set; }

        [Required(ErrorMessage = "NDA Start date is required.")]
        public DateTime NDAStartDate { get; set; }

        [Required(ErrorMessage = "NDA End date is required.")]
        public DateTime NDAEndDate { get; set; }

  


        [StringLength(1000, ErrorMessage = "Scope of work cannot exceed 1000 characters.")]
        public string? ScopeOfWork { get; set; }

        public string? IsRenewable { get; set; }

        [StringLength(50, ErrorMessage = "Owner type cannot exceed 50 characters.")]
        public string? OwnerType { get; set; }
     
        public double? DurationInYears { get; set; }

        public int? ThirdPartyId { get; set; }
        public virtual ThirdParty? ThirdParty { get; set; }

        public bool IsApproved { get; set; } 
       
    }

    public class UpdateOwnerTypeModel
    {
        public int Id { get; set; }
        public string? OwnerType { get; set; }
    }

}