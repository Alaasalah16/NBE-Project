using System.ComponentModel.DataAnnotations;

namespace NBE_Project.Models
{
    public class Incident
    {
       
            [Key]
            public int Id { get; set; } // Primary key
        /// <summary>
        /// ThirdParty
        /// </summary>
        //public ThirdParty? VendorName { get; set; }
        public int? ThirdPartyId { get; set; }
        public virtual ThirdParty? ThirdParty { get; set; }
        public string? IncidentNo { get; set; }
            public string? IncidentDescription { get; set; }
            public string? Severity { get; set; }
            public string? Status { get; set; }
            public DateTime IncidentDate { get; set; }
            public string? MitigationAction { get; set; }
            public string? Comment { get; set; }


        }
    }
