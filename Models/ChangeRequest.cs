using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NBE_Project.Models
{
    public class ChangeRequest
    {
       
            public int Id { get; set; }
    
        public int? ThirdPartyId { get; set; }
        public virtual ThirdParty? ThirdParty { get; set; }
        public string? Changes { get; set; }

            // Updates or notes related to the change request
            public string? Status { get; set; }

            // Date when the change is scheduled to be implemented
            public DateTime ScheduledDate { get; set; }

            // Additional comments or notes
            public string? Comment { get; set; }
            public string? ChangeSeverity { get; set; }
        //public ThirdParty? ThirdParty { get; set; }

        public bool IsApproved { get; set; }
        public bool IsDenied { get; set; }
    }
    }
