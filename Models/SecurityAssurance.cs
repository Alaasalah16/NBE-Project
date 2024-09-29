using System;
using System.ComponentModel.DataAnnotations;

namespace NBE_Project.Models
{
    public class SecurityAssurance
    {
        public int Id { get; set; }

        
        [MaxLength(20)]
        public string? VendorCategory { get; set; }

        public byte[]? SOCType2Report { get; set; }
        public string? SOCReportFileName { get; set; }

        
        public DateTime? SOCReportExpirationDate { get; set; }

     
        //thid party
        public int? ThirdPartyId { get; set; }
        public virtual ThirdParty? ThirdParty { get; set; }

        public bool IsApproved { get; set; } // Add if not present
        public bool IsExpired => SOCReportExpirationDate.HasValue && DateTime.Now > SOCReportExpirationDate.Value;


    }
}
