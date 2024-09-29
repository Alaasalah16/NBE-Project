using NBE_Project.Models;
using NBE_Project.Services;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Notifications;
using Microsoft.EntityFrameworkCore;

namespace NBE_Project.Controllers
{
    public class ThirdPartyController : Controller
    {
        private readonly ApplicationDBContext context;
        private readonly IWebHostEnvironment environment;
        private readonly IToastNotification _ToastNotification;

        public ThirdPartyController(ApplicationDBContext context, IWebHostEnvironment environment, IToastNotification toastNotification)
        {
            this.context = context;
            this.environment = environment;
            _ToastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            var thirdParties = context.ThirdParties.ToList();
            var companyCount = thirdParties.Count;
            var catCount = thirdParties.Count(o => o.VendorCategory == "Low");
            ViewBag.CompanyCount = companyCount;
            ViewBag.CatCount = catCount;
            return View(thirdParties);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ThirdPartyDto thirdPartyDto)
        {
            if (!ModelState.IsValid)
            {
                return View(thirdPartyDto);
            }

            // Handle file upload if ContractFile is provided
            string contractFilePath = "";
            if (thirdPartyDto.ContractFile != null)
            {
                var fileName = Path.GetFileName(thirdPartyDto.ContractFile.FileName);
                var uploadPath = Path.Combine(environment.WebRootPath, "uploads");

                // Ensure the "uploads" directory exists
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thirdPartyDto.ContractFile.CopyToAsync(stream);
                    }

                    contractFilePath = $"/uploads/{fileName}"; // Store relative path for contract file
                }
                catch (Exception ex)
                {
                    // Log the exception (optional)
                    ModelState.AddModelError("", "File upload failed. Please try again.");
                    return View(thirdPartyDto); // Return view with error message
                }
            }

            // Create the ThirdParty entity
            var party = new ThirdParty
            {
                VendorName = thirdPartyDto.VendorId,
                ContactTitle = thirdPartyDto.ContactTitle,
                Company = thirdPartyDto.Company,
                ContactNumber = thirdPartyDto.ContactNumber,
                Email = thirdPartyDto.Email,
                ContractName = contractFilePath, // Save the uploaded contract file path
                ContractStartDate = thirdPartyDto.ContractStartDate,
                ContractEndDate = thirdPartyDto.ContractEndDate
            };

            context.ThirdParties.Add(party);
            await context.SaveChangesAsync();

            // Create and add notifation for the new third party (intentionally keeping typo)
            var notifation = new Notifation
            {
                Message = $"New third party added. Check '{party.VendorName}' Profile",
                DateCreated = DateTime.Now,
                IsRead = false
            };

            context.Notifications.Add(notifation);
            await context.SaveChangesAsync();

            // Show success message
            _ToastNotification.AddSuccessToastMessage("You have completed your Profile");
            return RedirectToAction("VendorDetails", new { id = party.Id });
        }


        public IActionResult Detail(int id)
        {
            var part = context.ThirdParties.Find(id);
            if (part == null)
            {
                _ToastNotification.AddErrorToastMessage("Third party not found.");
                return RedirectToAction("Index");
            }

            var thirdPartyDto = new ThirdPartyDto
            {
                VendorId = part.VendorName,
                ContactTitle = part.ContactTitle,
                Company = part.Company,
                VendorCategory = part.VendorCategory,
                ContactNumber = part.ContactNumber,
                Email = part.Email,
                ContractName = part.ContractName,
                ContractStartDate = part.ContractStartDate,
                ContractEndDate = part.ContractEndDate
            };

            return View(thirdPartyDto);
        }



        // GET: Edit Third Party
        public IActionResult Edit(int id)
        {
            var part = context.ThirdParties.Find(id);
            if (part == null)
            {
                _ToastNotification.AddErrorToastMessage("Third party not found.");
                return RedirectToAction("Index");
            }

            var thirdPartyDto = new ThirdPartyDto
            {
                VendorId = part.VendorName,
                ContactTitle = part.ContactTitle,
                Company = part.Company,
                VendorCategory = part.VendorCategory,
                ContactNumber = part.ContactNumber,
                Email = part.Email,
                ContractName = part.ContractName, // Assuming ContractFile is a string path in DB
                ContractStartDate = part.ContractStartDate,
                ContractEndDate = part.ContractEndDate
                // No need to initialize ContractFile here as it's for file input
            };

            return View(thirdPartyDto);
        }

        // POST: Edit Third Party
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ThirdPartyDto thirdPartyDto)
        {
            if (!ModelState.IsValid)
            {
                return View(thirdPartyDto);
            }

            var part = await context.ThirdParties.FindAsync(id);
            if (part == null)
            {
                _ToastNotification.AddErrorToastMessage("Third party not found.");
                return RedirectToAction("Index");
            }

            // Handle file upload if a new file is provided
            if (thirdPartyDto.ContractFile != null)
            {
                // Validate file size, type, etc. (optional but recommended)
                if (thirdPartyDto.ContractFile.Length > 0)
                {
                    var fileName = Path.GetFileName(thirdPartyDto.ContractFile.FileName);
                    var filePath = Path.Combine(environment.WebRootPath, "uploads", fileName);

                    // Ensure the "uploads" directory exists
                    if (!Directory.Exists(Path.Combine(environment.WebRootPath, "uploads")))
                    {
                        Directory.CreateDirectory(Path.Combine(environment.WebRootPath, "uploads"));
                    }

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thirdPartyDto.ContractFile.CopyToAsync(stream);
                    }

                    // Update ContractName with the new file path
                    part.ContractName = $"/uploads/{fileName}";
                }
            }

            // Update other fields
            part.VendorName = thirdPartyDto.VendorId;
            part.ContactTitle = thirdPartyDto.ContactTitle;
            part.Company = thirdPartyDto.Company;
            part.VendorCategory = thirdPartyDto.VendorCategory;
            part.ContactNumber = thirdPartyDto.ContactNumber;
            part.Email = thirdPartyDto.Email;
            part.ContractStartDate = thirdPartyDto.ContractStartDate;
            part.ContractEndDate = thirdPartyDto.ContractEndDate;

            // Save changes to the database
            await context.SaveChangesAsync();

            // Add a success message and redirect
            _ToastNotification.AddInfoToastMessage("You updated a third party.");
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            try
            {
                var part = context.ThirdParties.Find(id);
                if (part == null)
                {
                    return RedirectToAction("Index");
                }

                context.ThirdParties.Remove(part);
                context.SaveChanges();

                var notification = new Notifation
                {
                    Message = $"Third party '{part.VendorName}' deleted.",
                    DateCreated = DateTime.Now,
                    IsRead = false
                };

                context.Notifications.Add(notification);
                context.SaveChanges();
                _ToastNotification.AddSuccessToastMessage("The third party has been successfully deleted.");
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using a logger service)
                _ToastNotification.AddErrorToastMessage("An error occurred while trying to delete the third party.");
                // Optionally log ex.Message or handle it based on your logging strategy
                return View("Error500");
            }
            return RedirectToAction("Index");
        }
    
        public IActionResult MarkNotificationsAsRead()
        {
            var unreadNotifications = context.Notifications.Where(n => !n.IsRead).ToList();
            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }
            context.SaveChanges();

            return Ok();
        }

   

        public async Task<IActionResult> VendorDetails(int id)
        {
            var vendor = await context.ThirdParties
                                      .Where(tp => tp.Id == id)
                                      .FirstOrDefaultAsync();

            if (vendor == null)
            {
                return NotFound(); // Return a 404 if the vendor is not found
            }

            var vendorDto = new ThirdPartyDto
            {
                // VendorId = vendor.Id,
                VendorId = vendor.VendorName,
                ContactTitle = vendor.ContactTitle,
                Company = vendor.Company,
                ContactNumber = vendor.ContactNumber,
                Email = vendor.Email,
                ContractName = vendor.ContractName,
                ContractStartDate = vendor.ContractStartDate,
                ContractEndDate = vendor.ContractEndDate
            };

            return View(vendorDto);
        }

    }
}
