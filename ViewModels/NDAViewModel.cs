using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using NBE_Project.Models;

namespace NBE_Project.ViewModels
{
    public class NDAViewModel
    {
        public int Id { get; set; }
        [NotMapped]
        public IFormFile? NDAFile { get; set; }
        public string? FileName { get; set; }

        [Required(ErrorMessage = "NDA Start date is required.")]
        public DateTime NDAStartDate { get; set; }

        [Required(ErrorMessage = "NDA End date is required.")]
        public DateTime NDAEndDate { get; set; }

        [StringLength(1000, ErrorMessage = "Scope of work cannot exceed 1000 characters.")]
        public string? ScopeOfWork { get; set; }

        public string? IsRenewable { get; set; }

        [StringLength(50, ErrorMessage = "Owner type cannot exceed 50 characters.")]
        public string? OwnerType { get; set; }
      //  public ThirdParty? VendorName { get; set; }
        public double? DurationInYears { get; set; }
        public List<SelectListItem>? OwnerTypes { get; set; }
        public int? ThirdPartyId { get; set; }
        public string? VendorName { get; set; }
        public List<SelectListItem> ThirdPartyList { get; set; } = new List<SelectListItem>();
        public bool IsApproved { get; set; }
      
    }
}