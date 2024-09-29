using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBE_Project.Models;
using NBE_Project.Services;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace NBE_Project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDBContext _context;

        public DashboardController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch and group third-party data by VendorCategory
            var thirdPartyData = await _context.ThirdParties.ToListAsync();

            var vendorCategories = thirdPartyData
                .GroupBy(tp => tp.VendorCategory)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .ToList();

            // Prepare data for JavaScript
            var categories = vendorCategories.Select(vc => vc.Category).ToList();
            var counts = vendorCategories.Select(vc => vc.Count).ToList();

            ViewBag.VendorCategories = JsonConvert.SerializeObject(categories);
            ViewBag.VendorCounts = JsonConvert.SerializeObject(counts);

            // Get the count of third-party vendors
            int thirdPartyCount = thirdPartyData.Count;
            ViewBag.ThirdPartyCount = thirdPartyCount;


            /////////////////////////////////////////////////////////////////
            ///


            // Fetch and process profile completion data
            var vendorData = _context.ThirdParties.ToList(); // Correct usage: Access as a property, not a method

            // Calculate completion data
            var profileCompletionData = vendorData.Select(v => new
            {
                VendorName = v.VendorName,
                CompletionPercentage = CalculateCompletionPercentage(v)
            }).ToList();

            // Prepare data for the chart
            var completionLabels = profileCompletionData.Select(p => p.VendorName).ToArray();
            var completionValues = profileCompletionData.Select(p => p.CompletionPercentage).ToArray();

            // Calculate summary information
            var totalVendors = vendorData.Count();
            var completedProfiles = profileCompletionData.Count(p => p.CompletionPercentage > 0);
            var incompleteProfiles = totalVendors - completedProfiles;

            // Create ViewBag properties to pass data to the view
            ViewBag.CompletionLabels = JsonConvert.SerializeObject(completionLabels);
            ViewBag.CompletionValues = JsonConvert.SerializeObject(completionValues);
            ViewBag.TotalVendors = totalVendors;
            ViewBag.CompletedProfiles = completedProfiles;
            ViewBag.IncompleteProfiles = incompleteProfiles;

            return View();
        }

        private int CalculateCompletionPercentage(ThirdParty tp)
        {
            int totalFields = 8;
            int completedFields = 1;

            if (!string.IsNullOrEmpty(tp.VendorCategory)) completedFields++;
            if (!string.IsNullOrEmpty(tp.ContactTitle)) completedFields++;
            if (!string.IsNullOrEmpty(tp.Company)) completedFields++;
            if (tp.ContactNumber > 0) completedFields++; // Check if ContactNumber is greater than 0
            if (!string.IsNullOrEmpty(tp.Email)) completedFields++;
            if (!string.IsNullOrEmpty(tp.ContractFile?.FileName)) completedFields++; // Assuming ContractFile is IFormFile
            if (tp.ContractStartDate != DateTime.MinValue) completedFields++; // For DateTime
            if (tp.ContractEndDate != DateTime.MinValue) completedFields++;

            return (completedFields * 100) / totalFields;
        }
    }
}
