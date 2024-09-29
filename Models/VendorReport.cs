namespace NBE_Project.Models
{
    public class VendorReport
    {
        public int ThirdPartyId { get; set; }
        public string? VendorName { get; set; }
        public NDAStatus? NDAStatus { get; set; }
        public SecurityAssuranceStatus? SecurityAssuranceStatus { get; set; }
        public IncidentStatus? IncidentStatus { get; set; }
        public AuditStatus? AuditStatus { get; set; }
        public ChangeRequestStatus? ChangeRequestStatus { get; set; }

        public int? NDAId { get; set; }
        public int? SecurityAssuranceId { get; set; }
        public int? IncidentId { get; set; }
        public int? AuditId { get; set; }
        public int? ChangeRequestId { get; set; }
        public int CompletionPercentage { get; set; }

        public virtual ThirdParty? ThirdParty { get; set; }


    }

}
    

        public enum NDAStatus
        {
            NotSubmitted,
            Submitted,
            Approved,
            Expired
        }

        public enum SecurityAssuranceStatus
        {
            NotSubmitted,
            Submitted,
            Approved,
            Expired
        }

        public enum IncidentStatus
        {
            NoIncidents,
            Open,
            Closed
        }

        public enum AuditStatus
        {
            NotScheduled,
            Scheduled,
            Completed
        }

        public enum ChangeRequestStatus
        {
            NotRequested,
            Requested,
            Approved,
            Denied
        }
    

