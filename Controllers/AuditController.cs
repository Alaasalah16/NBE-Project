using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;
using NBE_Project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace NBE_Project.Controllers
{
    public class AuditController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuditController(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

    

        public async Task<IActionResult> Index()
        {
            var auditLogs = await _context.Audit
                .Include(a => a.ThirdParty)
                 .OrderByDescending(a => a.FindingsDate)
                .ToListAsync();

            return View(auditLogs);
        }


        // Action to create a new audit log entry
        // GET: Awareness/Create
        public IActionResult Create()
        {
            // Fetch third party list from the database
            var thirdPartyList = _context.ThirdParties
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.VendorName
                })
                .ToList();

            // Check if the list is null and provide a fallback
            ViewBag.ThirdPartyList = thirdPartyList ?? new List<SelectListItem>
    {
        new SelectListItem { Value = "", Text = "No Vendors Available" }
    };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Audit model, IFormFile evidenceFile)
        {
            if (ModelState.IsValid)
            {
                if (evidenceFile != null && evidenceFile.Length > 0)
                {
                    // Define the path for the evidence files
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "evidenceFiles");

                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Path.GetFileName(evidenceFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await evidenceFile.CopyToAsync(stream);
                        }

                        model.EvidenceFileName = fileName;
                    }
                    catch (Exception ex)
                    {
                        // Handle file upload exceptions (log or display error)
                        ModelState.AddModelError("", $"File upload failed: {ex.Message}");
                        ViewBag.ThirdPartyList = _context.ThirdParties.Select(t => new SelectListItem
                        {
                            Value = t.Id.ToString(),
                            Text = t.VendorName
                        }).ToList();
                        return View(model);
                    }
                }

                // Add audit data to the database
                _context.Audit.Add(model);
                await _context.SaveChangesAsync();
                var notifation = new Notifation
                {
                    Message = $"New Audit  added. Check'Audit' Profile",
                    DateCreated = DateTime.Now,
                    IsRead = false
                };

                _context.Notifications.Add(notifation);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index","VendorHome");
            }

            // Re-populate the ViewBag if the model state is invalid
            ViewBag.ThirdPartyList = _context.ThirdParties.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.VendorName
            }).ToList();

            return View(model);
        }

        // GET: /Audit/Details/5
        public IActionResult Details(int id)
        {
            var audit = _context.Audit.SingleOrDefault(a => a.Id == id);
            if (audit == null)
            {
                return NotFound();
            }

            return View(audit);
        }
        // Action method to display audits by vendor
      
    }
}
