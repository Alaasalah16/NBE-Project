using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBE_Project.Models;
using NBE_Project.Services;
using NBE_Project.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class SecurityAssuranceController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ApplicationDBContext _context;

    public SecurityAssuranceController(IWebHostEnvironment hostingEnvironment, ApplicationDBContext context)
    {
        _hostingEnvironment = hostingEnvironment;
        _context = context;
    }
   
        public async Task<IActionResult> Index()
        {
            var model = await _context.SecurityAssurances
                .Include(sa => sa.ThirdParty) // Include related ThirdParty data
                .ToListAsync();
            return View(model);
        }

        [HttpGet]
        public JsonResult LoadVendorDetails(int thirdPartyId)
        {
            if (thirdPartyId <= 0)
            {
                return Json(new { vendorCategory = "" });
            }

            // Fetch the vendor category from the database based on the ThirdPartyId
            var vendorCategory = _context.ThirdParties
                .Where(tp => tp.Id == thirdPartyId)
                .Select(tp => tp.VendorCategory)
                .FirstOrDefault();

            if (vendorCategory == null)
            {
                return Json(new { vendorCategory = "" });
            }

            return Json(new { vendorCategory });
        }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new SecurityAssuranceVM();
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SecurityAssuranceVM model)
    {
        if (ModelState.IsValid)
        {
            if (model.ThirdPartyId == 0)
            {
                ModelState.AddModelError("ThirdPartyId", "Third Party ID is required.");
                return View(model);
            }

            var thirdParty = await _context.ThirdParties.FindAsync(model.ThirdPartyId);
            if (thirdParty == null)
            {
                ModelState.AddModelError("ThirdPartyId", "Invalid Third Party ID.");
                return View(model);
            }

            model.VendorCategory = thirdParty.VendorCategory;

            // Declare the securityAssurance variable
            var securityAssurance = new SecurityAssurance
            {
                VendorCategory = model.VendorCategory,
                SOCReportExpirationDate = model.SOCReportExpirationDate,
                ThirdPartyId = model.ThirdPartyId,
            };

            // Check if the VendorCategory is High or Questionable
            if (model.VendorCategory == "High" )
            {
                // Save the model in the index
                _context.SecurityAssurances.Add(securityAssurance);
                await _context.SaveChangesAsync();

                return RedirectToAction("Create", "Questionare");
            }

            // Handle file upload for Low and Medium categories
            if (model.VendorCategory == "Low" || model.VendorCategory == "Medium")
            {
                var SOCType2Report = Request.Form.Files.GetFile("SOCType2Report");
                if (SOCType2Report != null && SOCType2Report.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "soc-reports");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + SOCType2Report.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await SOCType2Report.CopyToAsync(fileStream);
                    }

                    securityAssurance.SOCReportFileName = uniqueFileName;

                    // Save SecurityAssurance for Low and Medium categories
                    _context.SecurityAssurances.Add(securityAssurance);
                    await _context.SaveChangesAsync();
                }
                else if (model.VendorCategory == "Medium")
                {
                    ModelState.AddModelError("", "SOC Type 2 Report is required for Medium category vendors.");
                    return View(model);
                }
            }

            return RedirectToAction("Index", "VendorHome");
        }

        return View(model);
    }
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(SecurityAssuranceVM model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        if (model.ThirdPartyId == 0)
    //        {
    //            ModelState.AddModelError("ThirdPartyId", "Third Party ID is required.");
    //            return View(model);
    //        }

    //        var thirdParty = await _context.ThirdParties.FindAsync(model.ThirdPartyId);
    //        if (thirdParty == null)
    //        {
    //            ModelState.AddModelError("ThirdPartyId", "Invalid Third Party ID.");
    //            return View(model);
    //        }

    //        model.VendorCategory = thirdParty.VendorCategory;

    //        if (model.VendorCategory == "High")
    //        {
    //            return RedirectToAction("Create", "Questionare", new { thirdPartyId = model.ThirdPartyId });
    //        }

    //        var securityAssurance = new SecurityAssurance
    //        {
    //            VendorCategory = model.VendorCategory,
    //            SOCReportExpirationDate = model.SOCReportExpirationDate,
    //            ThirdPartyId = model.ThirdPartyId,
    //        };

    //        // Handle file upload for Low and Medium categories
    //        if (model.VendorCategory == "Low" || model.VendorCategory == "Medium")
    //        {
    //            var SOCType2Report = Request.Form.Files.GetFile("SOCType2Report");
    //            if (SOCType2Report != null && SOCType2Report.Length > 0)
    //            {
    //                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "soc-reports");
    //                var uniqueFileName = Guid.NewGuid().ToString() + "_" + SOCType2Report.FileName;
    //                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

    //                Directory.CreateDirectory(uploadsFolder);

    //                using (var fileStream = new FileStream(filePath, FileMode.Create))
    //                {
    //                    await SOCType2Report.CopyToAsync(fileStream);
    //                }

    //                securityAssurance.SOCReportFileName = uniqueFileName;
    //            }
    //            else if (model.VendorCategory == "Medium")
    //            {
    //                ModelState.AddModelError("", "SOC Type 2 Report is required for Medium category vendors.");
    //                return View(model);
    //            }
    //        }

    //        _context.SecurityAssurances.Add(securityAssurance);
    //        await _context.SaveChangesAsync();

    //        return RedirectToAction("Index", "VendorHome");
    //    }

    //    return View(model);
    //}
    // Add this new method to get the VendorCategory
    [HttpGet]
    public async Task<IActionResult> GetVendorCategory(int thirdPartyId)
    {
        var thirdParty = await _context.ThirdParties.FindAsync(thirdPartyId);
        if (thirdParty == null)
        {
            return Json(new { vendorCategory = "" });
        }

        return Json(new { vendorCategory = thirdParty.VendorCategory });
    }
}









