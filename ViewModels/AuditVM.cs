using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NBE_Project.ViewModels
{
    public class AuditVM
    {
        public int? Id { get; set; }

       // public string? VendorName { get; set; }

        [Required(ErrorMessage = "Findings Name is required")]
        [StringLength(100, ErrorMessage = "Findings Name cannot exceed 100 characters")]
        public string? FindingsName { get; set; }

        [Display(Name = "Evidence File Name")]
        public string EvidenceFileName { get; set; } = "";
        [NotMapped]
        [Display(Name = "Upload Evidence")]
        public IFormFile? EvidenceFile { get; set; }

        [Required(ErrorMessage = "Findings Date is required.")]
        [Range(typeof(DateTime), "2024-01-01", "2050-01-01", ErrorMessage = "Date must be between January 1, 2024 and January 1, 2050")]
        [DataType(DataType.Date)]
        public DateTime FindingsDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string? Status { get; set; }

        [StringLength(200, ErrorMessage = "Action Taken cannot exceed 200 characters")]
        public string? ActionTaken { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }

        [StringLength(500, ErrorMessage = "Recommendation cannot exceed 500 characters")]
        public string? Recommendation { get; set; }
        public int? ThirdPartyId { get; set; }

        public string? VendorName { get; set; }


        public List<SelectListItem> ThirdPartyList { get; set; } = new List<SelectListItem>();
    }
}