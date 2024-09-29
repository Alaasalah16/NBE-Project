using Microsoft.AspNetCore.Mvc.Rendering;
using NBE_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace NBE_Project.ViewModels
{
    public class SecurityAssuranceVM
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string? VendorCategory { get; set; }

        public IFormFile? SOCType2Report { get; set; }
        public string? SOCReportFileName { get; set; }

        public DateTime? SOCReportExpirationDate { get; set; }

        public string? VendorName { get; set; }

        public int? ThirdPartyId { get; set; }
   
        public List<SelectListItem> ThirdPartyList { get; set; } = new List<SelectListItem>();
    }
}