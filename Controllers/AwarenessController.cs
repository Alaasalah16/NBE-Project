using NBE_Project.Models;
using NBE_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using NBE_Project.Services;
using MimeKit;

namespace NBE_Project.Controllers
{
    public class AwarenessController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IToastNotification _toastNotification;

        public AwarenessController(ApplicationDBContext context, IWebHostEnvironment environment, IToastNotification toastNotification)
        {
            _context = context;
            _environment = environment;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {

            var awarenessList = await _context.AwarenessMessages
                .Include(a => a.ThirdParty)
                .ToListAsync();

            // Return the list to the view
            return View(awarenessList);
        }

        // GET: Awareness/Create
        public void LogChange(string tableName, string username, string commandType, string changeSummary, string actionType)
        {
            var role = User.IsInRole("Admin") ? "Admin" : "Employee"; // Example of getting the role; modify as per your role system

            var log = new TransactionLog
            {
                TableName = tableName,
                Username = username,
                Role = role,
                CommandType = commandType,
                ActionType = actionType,
                ChangeSummary = changeSummary,
                ChangeDate = DateTime.Now
            };
            _context.TransactionLogs.Add(log);  // Add log to the context
            _context.SaveChanges();
        }
        /// <summary>
        /// //TRY Activity Log
        /// </summary>
        /// <returns></returns>
        public IActionResult RoleBasedLogs(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return View(new List<TransactionLog>());
            }

            var logs = _context.TransactionLogs
                               .Where(log => log.Role.Equals(role.Trim(), StringComparison.OrdinalIgnoreCase))
                               .OrderByDescending(log => log.ChangeDate)
                               .ToList();

            ViewBag.Role = role;

            return View(logs);
        }

        public IActionResult Create()
        {
            var thirdPartyList = _context.ThirdParties
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.VendorName
                })
                .ToList();

            // Ensure that thirdPartyList is not null or empty
            ViewBag.ThirdPartyList = thirdPartyList ?? new List<SelectListItem>();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AwarenessVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if file is provided
                    if (model.File == null || model.File.Length == 0)
                    {
                        ModelState.AddModelError("", "File content is required.");
                        ViewBag.ThirdPartyList = _context.ThirdParties.Select(t => new SelectListItem
                        {
                            Value = t.Id.ToString(),
                            Text = t.VendorName
                        }).ToList();
                        return View(model);
                    }

                    // Save the file to disk
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "files");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fileStream);
                    }

                    // Create the awareness message
                    var awareness = new Awareness
                    {
                        FileName = uniqueFileName,
                        Message = model.Message,
                        ThirdPartyId = model.ThirdPartyId
                    };

                    _context.AwarenessMessages.Add(awareness);
                    await _context.SaveChangesAsync();

                    _toastNotification.AddSuccessToastMessage("Awareness message uploaded successfully.");

                    // Log the change
                    LogChange("Awareness", User.Identity.Name, "Insert", $"Added Awareness: {model.Message}", "Send Awareness");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _toastNotification.AddErrorToastMessage($"An error occurred: {ex.Message}");
                }
            }

            // Re-populate dropdown list if validation fails
            ViewBag.ThirdPartyList = _context.ThirdParties.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.VendorName
            }).ToList();

            return View(model);
        }



        public async Task<IActionResult> Details(int id)
        {

            var awareness = await _context.AwarenessMessages
                .Include(a => a.ThirdParty)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (awareness == null)
            {
                return NotFound();
            }


            return View(awareness);
        }



        public IActionResult VendorIndex(int vendorId)
        {
            var recentAwareness = _context.AwarenessMessages
                .Where(a => a.ThirdPartyId == vendorId)
                .OrderByDescending(a => a.Id)
                .FirstOrDefault();

            var awarenessMessages = _context.AwarenessMessages
                .Where(a => a.ThirdPartyId == vendorId)
                .OrderByDescending(a => a.Id)
                .Select(a => new AwarenessVM
                {
                    Id = a.Id,
                    Message = a.Message,
                    FileName = a.FileName
                }).ToList();

            var viewModel = new AwarenessVM
            {
                RecentAwareness = recentAwareness,
                AwarenessMessages = awarenessMessages
            };

            ViewBag.VendorId = vendorId; // Ensure vendorId is passed to the view
            return View(viewModel);
        }

        // GET: Awareness RecentMessage
        public IActionResult RecentMessage(int vendorId)
        {
            var recentAwareness = _context.AwarenessMessages
                .Where(a => a.ThirdPartyId == vendorId)
                .OrderByDescending(a => a.Id)
                .FirstOrDefault();

            var awarenessMessages = _context.AwarenessMessages
                .Where(a => a.ThirdPartyId == vendorId)
                .OrderByDescending(a => a.Id)
                .Select(a => new AwarenessVM
                {
                    Id = a.Id,
                    Message = a.Message,
                    FileName = a.FileName
                }).ToList();

            var viewModel = new AwarenessVM
            {
                RecentAwareness = recentAwareness,
                AwarenessMessages = awarenessMessages
            };

            ViewBag.VendorId = vendorId;
            return View(viewModel);
        }

        // Method to retrieve awareness messages
        private IEnumerable<AwarenessVM> GetAwarenessMessages()
        {
            return _context.AwarenessMessages
                .Select(message => new AwarenessVM
                {
                    Id = message.Id,
                    FileName = message.FileName,
                    Message = message.Message,
                    ThirdPartyId = message.ThirdPartyId
                }).ToList();
        }
        // DELETE: Awareness/Delete/5
  
        public async Task<IActionResult> Delete(int id)
        {
            // Find the awareness message by ID
            var awareness = await _context.AwarenessMessages.FindAsync(id);

            if (awareness == null)
            {
                // Show error notification if not found
                _toastNotification.AddErrorToastMessage("Awareness message not found.");
                return NotFound();
            }

            try
            {
                // Remove the awareness message from the context
                _context.AwarenessMessages.Remove(awareness);
                await _context.SaveChangesAsync();

                // Log the change
                LogChange("Awareness", User.Identity.Name, "Delete", $"Deleted Awareness: {awareness.Message}", "Delete Awareness");

                // Show success notification
                _toastNotification.AddSuccessToastMessage("Awareness message deleted successfully.");
              //  LogChange("Awareness", User.Identity.Name, "Dalete", $"Remove Awareness: {model.Message}", "Send Awareness");

                // Redirect to Index
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Show error message in case of failure
                _toastNotification.AddErrorToastMessage($"An error occurred: {ex.Message}");
                return View("Error");
            }
        }

    }
}
   
