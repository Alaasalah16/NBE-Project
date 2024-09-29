using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBE_Project.Models;
using NBE_Project.Services;
using NBE_Project.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace NBE_Project.Controllers
{
    public class RegulationComplianceController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RegulationComplianceController(ApplicationDBContext context)
        {
            _context = context;
        }
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
        public IActionResult TransactionLogs()
        {
            var logs = _context.TransactionLogs.ToList();  // Retrieve logs from the database
            ViewBag.Role = User.IsInRole("Admin") ? "Admin" : "Employee";  // Set role in ViewBag
            return View(logs);  // Pass logs to the view
        }

        // GET: RegulationCompliance/Create
        public IActionResult Create()
        {
            var viewModel = new RegulationComplianceVM
            {
                ThirdPartyList = _context.ThirdParties
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.VendorName
                    })
                    .ToList(),
                Clauses = new List<ClauseVM>() // Empty list for clauses on create
            };

            return View(viewModel);
        }

        // POST: RegulationCompliance/Create


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegulationComplianceVM model)
        {
            if (!ModelState.IsValid)
            {
                // Reload third-party list if there's an error
                model.ThirdPartyList = _context.ThirdParties
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.VendorName
                    })
                    .ToList();

                return View(model);
            }

            if (model.Clauses == null || !model.Clauses.Any())
            {
                ModelState.AddModelError("", "No clauses found in the submission.");
                return View(model);
            }

            // Map ViewModel to the actual data model
            var regulationCompliance = new RegulationCompliance
            {
                ThirdPartyId = model.ThirdPartyId,
                RegulationName = model.RegulationName,
                ComplianceDate = model.ComplianceDate,
                Comments = model.Comments,
                Clauses = model.Clauses.Select(c => new Clause
                {
                    ClauseName = c.ClauseName,
                    Requirements = c.Requirements,
                    CompilationStatus = c.CompilationStatus,
                    Comments = c.Comments
                }).ToList()
            };

            _context.RegulationCompliances.Add(regulationCompliance);
            await _context.SaveChangesAsync();
            // Log the change
             LogChange("RegulationCompliance", User.Identity.Name, "Insert", $"Added Reg: {model.VendorName}", "Send Reg");

            return RedirectToAction(nameof(Index));
        }
        // GET: RegulationCompliance/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var regulationCompliance = await _context.RegulationCompliances
                .Include(r => r.Clauses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (regulationCompliance == null)
            {
                return NotFound();
            }

            return View(regulationCompliance);
        }

      
            // GET: RegulationCompliance/Edit/5
            public async Task<IActionResult> Edit(int id)
        {
            var regulationCompliance = await _context.RegulationCompliances
                .Include(r => r.Clauses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (regulationCompliance == null)
            {
                return NotFound();
            }

            var viewModel = new RegulationComplianceVM
            {
                Id = regulationCompliance.Id,
                ThirdPartyId = regulationCompliance.ThirdPartyId,
                RegulationName = regulationCompliance.RegulationName,
                ComplianceDate = regulationCompliance.ComplianceDate,
                Comments = regulationCompliance.Comments,
                Clauses = regulationCompliance.Clauses.Select(c => new ClauseVM
                {
                    ClauseName = c.ClauseName,
                    Requirements = c.Requirements,
                    CompilationStatus = c.CompilationStatus,
                    Comments = c.Comments
                }).ToList(),
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

        // POST: RegulationCompliance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RegulationComplianceVM model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                model.ThirdPartyList = _context.ThirdParties
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.VendorName
                    })
                    .ToList();

                return View(model);
            }

            var regulationCompliance = await _context.RegulationCompliances
                .Include(r => r.Clauses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (regulationCompliance == null)
            {
                return NotFound();
            }

            regulationCompliance.ThirdPartyId = model.ThirdPartyId;
            regulationCompliance.RegulationName = model.RegulationName;
            regulationCompliance.ComplianceDate = model.ComplianceDate;
            regulationCompliance.Comments = model.Comments;

            // Update Clauses
            regulationCompliance.Clauses = model.Clauses.Select(c => new Clause
            {
                Id = c.Id,
                ClauseName = c.ClauseName,
                Requirements = c.Requirements,
                CompilationStatus = c.CompilationStatus,
                Comments = c.Comments,
                RegulationComplianceId = regulationCompliance.Id
            }).ToList();

            try
            {
                _context.Update(regulationCompliance);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegulationComplianceExists(model.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: RegulationCompliance/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var regulationCompliance = await _context.RegulationCompliances
                .FirstOrDefaultAsync(m => m.Id == id);

            if (regulationCompliance == null)
            {
                return NotFound();
            }

            _context.RegulationCompliances.Remove(regulationCompliance);
            await _context.SaveChangesAsync();
            LogChange("RegulationCompliance", User.Identity.Name, "Delete", $"Remvove Reg: ", " Reg Deleted");


            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if RegulationCompliance exists
        private bool RegulationComplianceExists(int id)
        {
            return _context.RegulationCompliances.Any(e => e.Id == id);
        }

        // Fetch the most recent regulation compliance message
        public IActionResult RecentCompliance(int thirdPartyId)
        {
            var recentCompliance = _context.RegulationCompliances
                .Where(rc => rc.ThirdPartyId == thirdPartyId)
                .OrderByDescending(rc => rc.ComplianceDate)
                .FirstOrDefault();

            if (recentCompliance == null)
            {
                return NotFound(); // Handle no message found
            }

            var viewModel = new RegulationComplianceVM
            {
                Id = recentCompliance.Id,
                ThirdPartyId = recentCompliance.ThirdPartyId,
                RegulationName = recentCompliance.RegulationName,
                ComplianceDate = recentCompliance.ComplianceDate,
                Comments = recentCompliance.Comments,
                Clauses = recentCompliance.Clauses.Select(c => new ClauseVM
                {
                    Id = c.Id,
                    ClauseName = c.ClauseName,
                    Requirements = c.Requirements,
                    CompilationStatus = c.CompilationStatus,
                    Comments = c.Comments
                }).ToList()
            };

            return View(viewModel);
        }

        // List all compliance messages for a third party
        public IActionResult VendorIndex(int thirdPartyId)
        {
            if (thirdPartyId == 0)
            {
                // Handle case where thirdPartyId is not provided or is invalid
                ViewBag.ErrorMessage = "Invalid third party ID.";
                return View(new List<RegulationComplianceVM>()); // Return empty view model
            }

            var complianceMessages = _context.RegulationCompliances
                .Where(rc => rc.ThirdPartyId == thirdPartyId)
                .OrderByDescending(rc => rc.ComplianceDate)
                .Select(rc => new RegulationComplianceVM
                {
                    Id = rc.Id,
                    ThirdPartyId = rc.ThirdPartyId,
                    RegulationName = rc.RegulationName,
                    ComplianceDate = rc.ComplianceDate,
                    Comments = rc.Comments
                })
                .ToList();

            if (!complianceMessages.Any())
            {
                // Handle case where no messages are found for the provided thirdPartyId
                ViewBag.Message = "No compliance messages found for this third-party ID.";
            }

            ViewBag.ThirdPartyId = thirdPartyId;
            return View(complianceMessages);
        }
        // GET: RegulationCompliance/Index
        public async Task<IActionResult> Index()
        {
            var data = await _context.RegulationCompliances
                .Include(rc => rc.Clauses)
                .Include(rc => rc.ThirdParty) // Include the third-party vendor details
                .Select(rc => new RegulationComplianceVM
                {
                    Id = rc.Id,
                    RegulationName = rc.RegulationName,
                    ComplianceDate = rc.ComplianceDate,
                    Comments = rc.Comments,
                    VendorName = rc.ThirdParty != null ? rc.ThirdParty.VendorName : "No Vendor", // Handle null vendor
                    Clauses = rc.Clauses.Select(c => new ClauseVM
                    {
                        ClauseName = c.ClauseName,
                        Requirements = c.Requirements,
                        CompilationStatus = c.CompilationStatus,
                        Comments = c.Comments
                    }).ToList()
                })
                .ToListAsync();

            return View(data); // Pass the list to the view
        }
        public async Task<IActionResult> VendorDetails(int id)
        {
            var regulationCompliance = await _context.RegulationCompliances
                .Include(r => r.Clauses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (regulationCompliance == null)
            {
                return NotFound();
            }

            return View(regulationCompliance);
        }
        [HttpPost]
        public IActionResult SubmitFeedback(int regulationId, string compilationStatus, string feedback)
        {
            // Fetch the regulation compliance entry by id
            var regulationCompliance = _context.RegulationCompliances.Find(regulationId);
            if (regulationCompliance == null)
            {
                return NotFound();
            }

            // Update compilation status and feedback
            regulationCompliance.CompilationStatus = compilationStatus;
            regulationCompliance.Feedback = feedback;

            // Save changes
            _context.SaveChanges();

            // Redirect or return to the view
            return RedirectToAction("VendorIndex");
        }


    }

}

