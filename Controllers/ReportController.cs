using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;
using NBE_Project.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using NBE_Project.ViewModels;

namespace NBE_Project.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ReportController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var thirdParties = await _context.ThirdParties.ToListAsync();
            return View(thirdParties);
        }

        [Route("Report/VendorReport/{thirdPartyId}")]
        public async Task<IActionResult> VendorReport(int thirdPartyId)
        {
            var vendor = await _context.ThirdParties.FindAsync(thirdPartyId);
            if (vendor == null)
            {
                return NotFound();
            }

            var viewModel = new ReportViewModel
            {
                Vendor = vendor,
                NDAs = await _context.NDAs.Where(n => n.ThirdPartyId == thirdPartyId).ToListAsync(),
                Incidents = await _context.Incidents.Where(i => i.ThirdPartyId == thirdPartyId).ToListAsync(),
                Audits = await _context.Audit.Where(a => a.ThirdPartyId == thirdPartyId).ToListAsync(),
                ChangeRequests = await _context.ChangeRequests.Where(c => c.ThirdPartyId == thirdPartyId).ToListAsync()
            };

            return View(viewModel);
        }
    }
 
}
    //private int CalculateCompletionPercentage(int thirdPartyId)
    //    {
    //        int totalFields = 5; // Total categories to check
    //        int completedFields = 0;

    //        var ndaStatus = GetNDAStatusAsync(thirdPartyId).Result;
    //        var securityAssuranceStatus = GetSecurityAssuranceStatusAsync(thirdPartyId).Result;
    //        var incidentStatus = GetIncidentStatusAsync(thirdPartyId).Result;
    //        var auditStatus = GetAuditStatusAsync(thirdPartyId).Result;
    //        var changeRequestStatus = GetChangeRequestStatusAsync(thirdPartyId).Result;

    //        if (ndaStatus != NDAStatus.NotSubmitted) completedFields++;
    //        if (securityAssuranceStatus != SecurityAssuranceStatus.NotSubmitted) completedFields++;
    //        if (incidentStatus != IncidentStatus.NoIncidents) completedFields++;
    //        if (auditStatus != AuditStatus.NotScheduled) completedFields++;
    //        if (changeRequestStatus != ChangeRequestStatus.NotRequested) completedFields++;

    //        return (completedFields * 100) / totalFields;
    //    }


