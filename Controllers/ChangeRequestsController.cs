using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;
using NBE_Project.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using NBE_Project.Services;

namespace NBE_Project.Controllers
{
    public class ChangeRequestsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ChangeRequestsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Action to display the list of change requests
        // Action to display the list of change requests with third party (vendor) info
        public async Task<IActionResult> Index()
        {
            var changeRequests = await _context.ChangeRequests
                .Include(cr => cr.ThirdParty)  // Include ThirdParty (vendor) information
                .OrderByDescending(cr => cr.ScheduledDate) // Order by scheduled date
                .Select(cr => new ChangeRequestVM
                {
                    Id = cr.Id,
                    VendorName = cr.ThirdParty != null ? cr.ThirdParty.VendorName : "No Vendor", // Handle null vendor
                    Changes = cr.Changes,
                    Status = cr.Status,
                    ScheduledDate = cr.ScheduledDate,
                    Comment = cr.Comment,
                    ChangeSeverity = cr.ChangeSeverity
                })
                .ToListAsync();

            return View(changeRequests); // Pass the list to the view
        }

        // GET: ChangeRequests/Create
        // GET: ChangeRequests/Create
        [HttpGet]
        public IActionResult Create()
        {
            var thirdPartyList = _context.ThirdParties
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.VendorName
                })
                .ToList();

            // Ensure ViewBag.ThirdPartyList is always populated, even if the list is empty
            ViewBag.ThirdPartyList = thirdPartyList.Any() ? thirdPartyList : new List<SelectListItem>
    {
        new SelectListItem { Value = "", Text = "No Vendors Available" }
    };

            return View(new ChangeRequestVM());
        }

        // POST: ChangeRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChangeRequestVM model)
        {
            if (ModelState.IsValid)
            {
                var changeRequest = new ChangeRequest
                {
                    Changes = model.Changes,
                    Status = model.Status,
                    ScheduledDate = model.ScheduledDate,
                    Comment = model.Comment,
                    ChangeSeverity = model.ChangeSeverity,
                    ThirdPartyId = model.ThirdPartyId
                    //   VendorName = model.ThirdPartyId.HasValue ? await _context.ThirdParties.FindAsync(model.ThirdPartyId) : null
                };

                _context.ChangeRequests.Add(changeRequest);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index","VendorHome");
            }

            // Repopulate dropdown if needed for validation errors
          
            ViewBag.ThirdPartyList = new SelectList(_context.ThirdParties, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeverity([FromBody] ChangeRequest model)
        {
            if (ModelState.IsValid)
            {
                var request = await _context.ChangeRequests.FindAsync(model.Id);
                if (request != null)
                {
                    request.ChangeSeverity = model.ChangeSeverity;
                    _context.Update(request);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Severity updated successfully." });
                }
                return Json(new { success = false, message = "Request not found." });
            }
            return Json(new { success = false, message = "Invalid data." });
        }


        // GET: ChangeRequests/Details/5
        public IActionResult Details(int id)
        {
            var changeRequest = _context.ChangeRequests
                .Where(cr => cr.Id == id)
                .Select(cr => new ChangeRequestVM
                {
                    Id = cr.Id,
                    VendorName = cr.ThirdParty.VendorName ?? "", // Handle nullable string
                    VendorCategory = cr.ThirdParty.VendorCategory ?? "",
                    Company = cr.ThirdParty.Company ?? "",
                    Changes = cr.Changes ?? "", // Handle nullable string
                    Status = cr.Status ?? "",
                    ScheduledDate = cr.ScheduledDate,
                    Comment = cr.Comment ?? "", // Handle nullable string
                    ChangeSeverity = cr.ChangeSeverity ?? "" // Handle nullable string
                })
                .FirstOrDefault();

            if (changeRequest == null)
            {
                return NotFound();
            }

            return View(changeRequest);
        }
       

    }
}

   
