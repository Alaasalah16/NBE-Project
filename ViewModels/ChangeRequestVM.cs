using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NBE_Project.ViewModels
{
    public class ChangeRequestVM
    {
        public int Id { get; set; }


       
        public string VendorCategory { get; set; } = "";
        public string Company { get; set; } = "";

        [Required(ErrorMessage = "Change description is required.")]
        [StringLength(500, ErrorMessage = "Changes description cannot exceed 500 characters.")]
        public string? Changes { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(100, ErrorMessage = "Status cannot exceed 100 characters.")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Scheduled Date is required.")]
        [Range(typeof(DateTime), "2024-01-01", "2050-01-01", ErrorMessage = "Date must be between January 1, 2024 and January 1, 2050")]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; } = DateTime.Now;

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string? Comment { get; set; }
        public string? ChangeSeverity { get; set; }
        public int? ThirdPartyId { get; set; }
        public string? VendorName { get; set; } = "";

        public List<SelectListItem> ThirdPartyList { get; set; } = new List<SelectListItem>();
    }
}


