using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBE_Project.Models;
using NBE_Project.Services;
using NBE_Project.ViewModels;
using NToastNotify;


namespace NBE_Project.Controllers
{
    public class IncidentController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly INotificationService _notificationService;
        private readonly IToastNotification _toastNotification;

        public IncidentController(ApplicationDBContext context, INotificationService notificationService, IToastNotification toastNotification)
        {
            _context = context;
            _notificationService = notificationService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var incidents = await _context.Incidents
                    .Include(a => a.ThirdParty)  
                    .ToListAsync();

            return View(incidents);
        }
        // GET: /Incident/Create
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

            // Populate ViewBag or ViewData to pass the list to the view
            ViewBag.ThirdPartyList = thirdPartyList ?? new List<SelectListItem>
    {
        new SelectListItem { Value = "", Text = "No Vendors Available" }
    };

            return View();
        }


        // POST: /Incident/Create
        [HttpPost]
        public async Task<IActionResult> Create(IncidentVM model)
        {
            if (ModelState.IsValid)
            {
                var incident = new Incident
                {
                    IncidentNo = model.IncidentNo,
                    IncidentDescription = model.IncidentDescription,
                    Severity = model.Severity,
                    Status = model.Status,
                    IncidentDate = model.IncidentDate,
                    MitigationAction = model.MitigationAction,
                    Comment = model.Comment,
                    ThirdPartyId = model.ThirdPartyId
                };

                _context.Incidents.Add(incident);
                await _context.SaveChangesAsync();

                if (model.Severity == "high")
                {
                  _notificationService.NotifyKeyResponseTeam(incident);
                }

                _toastNotification.AddSuccessToastMessage("A new incident has been successfully added.");
                return RedirectToAction("Index","VendorHome");
            }

            // Re-populate ThirdPartyList in case of validation failure
            ViewData["ThirdPartyList"] = await _context.ThirdParties
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.VendorName
                })
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            return View(incident);
        }
       
    }
}