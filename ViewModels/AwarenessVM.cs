using Microsoft.AspNetCore.Mvc.Rendering;
using NBE_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace NBE_Project.ViewModels
{
    public class AwarenessVM
    {
        public int? Id { get; set; }

        public int? ThirdPartyId { get; set; }

        public string? VendorName { get; set; }


        [StringLength(500, ErrorMessage = "The message cannot be longer than 500 characters.")]
        public string? Message { get; set; }

        [Display(Name = "Upload File")]
        public IFormFile? File { get; set; }  // This handles file uploads in the form

        public string? FileName { get; set; }  // Stores the name of the uploaded file
        public byte[]? FileContent { get; set; }  // Stores the content of the uploaded file after processing
        public IEnumerable<AwarenessVM>? AwarenessMessages { get; set; }
        public Awareness? RecentAwareness { get; set; } // Add this property

       
        public List<SelectListItem> ThirdPartyList { get; set; } = new List<SelectListItem>();

    }
}