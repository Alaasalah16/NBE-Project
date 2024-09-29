using Microsoft.AspNetCore.Mvc.Rendering;
using NBE_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace NBE_Project.ViewModels
{
    public class IncidentVM
    {
        public int Id { get; set; } // Primary key

        // Navigation property for ThirdParty
       // public ThirdParty? VendorName { get; set; }

        [Required(ErrorMessage = "Incident number is required.")]
        [StringLength(50, ErrorMessage = "Incident number can't be longer than 50 characters.")]
        public string? IncidentNo { get; set; }

        [Required(ErrorMessage = "Incident description is required.")]
        public string? IncidentDescription { get; set; }

        [Required(ErrorMessage = "Severity is required.")]
        [StringLength(20, ErrorMessage = "Severity can't be longer than 20 characters.")]
        public string? Severity { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status can't be longer than 20 characters.")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Incident date is required.")]
        [DataType(DataType.Date)]
        public DateTime IncidentDate { get; set; }

        [StringLength(500, ErrorMessage = "Mitigation action can't be longer than 500 characters.")]
        public string? MitigationAction { get; set; }

        [StringLength(500, ErrorMessage = "Comment can't be longer than 500 characters.")]
        public string? Comment { get; set; }
        public int? ThirdPartyId { get; set; }
        public string? VendorName { get; set; }
   public List<SelectListItem> ThirdPartyList { get; set; } = new List<SelectListItem>();
      
    }
}