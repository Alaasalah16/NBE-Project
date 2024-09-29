using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;
using NBE_Project.ViewModels;
using NBE_Project.Services;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NBE_Project.Controllers
{
    public class NDAController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly INotificationService _notificationService;

        public NDAController(ApplicationDBContext context, IToastNotification toastNotification, INotificationService notificationService)
        {
            _context = context;
            _toastNotification = toastNotification;
            _notificationService = notificationService;
        }

        // GET: /NDA/Upload
        public IActionResult Upload()
        {
            var viewModel = new NDAViewModel
            {
                ThirdPartyList = _context.ThirdParties
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.VendorName
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: /NDA/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(NDAViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.NDAFile == null || model.NDAFile.Length == 0)
                    {
                        ModelState.AddModelError("", "File content is required.");
                        return View(model);
                    }

                    byte[] fileContent;
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.NDAFile.CopyToAsync(memoryStream);
                        fileContent = memoryStream.ToArray();
                    }

                    var nda = new NDA
                    {
                        FileName = model.NDAFile.FileName,
                        NDAStartDate = model.NDAStartDate,
                        NDAEndDate = model.NDAEndDate,
                        ScopeOfWork = model.ScopeOfWork,
                        IsRenewable = model.IsRenewable,
                        OwnerType = model.OwnerType,
                        ThirdPartyId = model.ThirdPartyId,
                        IsApproved = false, 
                    };

                    _context.NDAs.Add(nda);

                    var notification = new Notifation
                    {
                        Message = $"New Vendor NDA added. Check 'NDA' Profile.",
                        DateCreated = DateTime.Now,
                        IsRead = false
                    };

                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();

                    _toastNotification.AddSuccessToastMessage("NDA uploaded successfully.");
                    return RedirectToAction("Index", "VendorHome");
                }
                catch (Exception ex)
                {
                    _toastNotification.AddErrorToastMessage($"An error occurred: {ex.Message}");
                }
            }

            // Reload third-party list in case of validation failure
            model.ThirdPartyList = _context.ThirdParties
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.VendorName
                })
                .ToList();

            return View(model);
        }

        // Update GetNDAs to include ThirdParty data
        private IEnumerable<NDAViewModel> GetNDAs()
        {
            return _context.NDAs
                .Include(nda => nda.ThirdParty) // Include third-party data
                .Select(nda => new NDAViewModel
                {
                    Id = nda.Id,
                    FileName = nda.FileName,
                    NDAStartDate = nda.NDAStartDate,
                    NDAEndDate = nda.NDAEndDate,
                    ScopeOfWork = nda.ScopeOfWork,
                    IsRenewable = nda.IsRenewable,
                    DurationInYears = (nda.NDAEndDate - nda.NDAStartDate).TotalDays / 365.25,
                    VendorName = nda.ThirdParty != null ? nda.ThirdParty.VendorName : "No Vendor",
                    IsApproved = nda.IsApproved,
                   
                })
                .ToList();
        }

        // GET: /NDA/Index
        public IActionResult Index()
        {
            var ndas = GetNDAs();
            ViewBag.OwnerTypes = new List<SelectListItem>
    {
        new SelectListItem { Value = "IT Owner", Text = "IT Owner" },
        new SelectListItem { Value = "Business Owner", Text = "Business Owner" }
    };

            return View(ndas);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOwnerType([FromBody] UpdateOwnerTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var nda = await _context.NDAs.FindAsync(model.Id);
                if (nda != null)
                {
                    nda.OwnerType = model.OwnerType;
                    _context.Update(nda);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Owner Type updated successfully." });
                }
                return Json(new { success = false, message = "NDA not found." });
            }
            return Json(new { success = false, message = "Invalid data." });
        }



        [HttpGet]
        public IActionResult Details(int id)
        {
            var nda = _context.NDAs.FirstOrDefault(n => n.Id == id);
            if (nda == null)
            {
                return NotFound();
            }

            var ndaViewModel = new NDAViewModel
            {
                Id = nda.Id,
                NDAStartDate = nda.NDAStartDate,
                NDAEndDate = nda.NDAEndDate,
                IsRenewable = nda.IsRenewable,
                ScopeOfWork = nda.ScopeOfWork,
                OwnerType = nda.OwnerType,
                FileName = nda.FileName,
                //IsApproved = nda.IsApproved,
            };

            return View(ndaViewModel);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var nda = await _context.NDAs.FindAsync(id);
            if (nda == null)
            {
                return NotFound();
            }

            nda.IsApproved = true; 
            _context.NDAs.Update(nda);
            await _context.SaveChangesAsync();

            _toastNotification.AddSuccessToastMessage("NDA approved successfully.");
            return RedirectToAction("Index");
        }

    }
}
