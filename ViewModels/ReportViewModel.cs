using NBE_Project.Models;

namespace NBE_Project.ViewModels
{
    public class ReportViewModel
    {
        public ThirdParty? Vendor { get; set; }
        public List<NDA>? NDAs { get; set; }
        public List<Incident>? Incidents { get; set; }
        public List<Audit>? Audits { get; set; }
        public List<ChangeRequest>? ChangeRequests { get; set; }
    }
}
