using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;
using NBE_Project.Services;
using System.Data;

namespace NBE_Project.Controllers
{
    public class ActivitiesController : Controller
    {
      
            private readonly ApplicationDBContext _context;
            private readonly IWebHostEnvironment _webHostEnvironment;

            // Constructor to initialize _context and _webHostEnvironment
            public ActivitiesController(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
            {
                _context = context;
                _webHostEnvironment = webHostEnvironment;
            }

            // Method to log changes
            public void LogChange(string tableName,string username,string commandType, string changeSummary, string actionType)
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

                _context.TransactionLogs.Add(log);
                _context.SaveChanges(); // Ensure _context is initialized correctly
            }
        public IActionResult RoleBasedLogs(string role = null)
        {
            // Fetch all logs without filtering if no role is provided
            var logs = _context.TransactionLogs.AsQueryable();

            // If a role is provided, filter the logs by role
            if (!string.IsNullOrEmpty(role))
            {
                logs = logs.Where(log => log.Role.Trim().Equals(role.Trim(), StringComparison.OrdinalIgnoreCase));
                ViewBag.Role = role;
            }
            else
            {
                ViewBag.Role = "All Logs";
            }

            var logsList = logs.OrderByDescending(log => log.ChangeDate).ToList();

            return View(logsList);
        }
        [Route("Activities/Notification")]
            public IActionResult Notification()
            {
                return View();
            }

        }
    }
