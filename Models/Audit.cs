using System.ComponentModel.DataAnnotations.Schema;

namespace NBE_Project.Models
{
    public class Audit
    {
        public int? Id { get; set; }

        public int? ThirdPartyId { get; set; }  // Foreign Key
        public virtual ThirdParty? ThirdParty { get; set; }  // Navigation Property

        public string? FindingsName { get; set; }

        public string? EvidenceFileName { get; set; } = "";
        [NotMapped]
        public IFormFile? EvidenceFile { get; set; }

        public DateTime FindingsDate { get; set; }

        public string? Status { get; set; }

        public string? ActionTaken { get; set; }

        public string? Comment { get; set; }

        public string? Recommendation { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsScheduled => FindingsDate > DateTime.Now;
    }
    }

