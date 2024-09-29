using Microsoft.AspNetCore.Mvc;
using NBE_Project.Services;
using NBE_Project.ViewModels;
using NBE_Project.Models;

namespace NBE_Project.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public SettingsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Settings
        public IActionResult Index()
        {
            var model = _context.Settings.FirstOrDefault() ?? new Settings();
            var viewModel = new SettingsViewModel
            {
                Theme = model.Theme,
                NotificationsEnabled = model.NotificationsEnabled ?? false,
                Language = model.Language,
                TwoFactorAuthEnabled = model.TwoFactorAuthEnabled ?? false,
                DatabaseConnectionString = model.DatabaseConnectionString,
                ApiEndpoint = model.ApiEndpoint,
                ComplianceThreshold = model.ComplianceThreshold
            };

            return View(viewModel);
        }

        // POST: Settings
        [HttpPost]
        public IActionResult SaveSettings(SettingsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var settings = _context.Settings.FirstOrDefault() ?? new Settings();
                settings.Theme = viewModel.Theme;
                settings.NotificationsEnabled = viewModel.NotificationsEnabled;
                settings.Language = viewModel.Language;
                settings.TwoFactorAuthEnabled = viewModel.TwoFactorAuthEnabled;
                settings.DatabaseConnectionString = viewModel.DatabaseConnectionString;
                settings.ApiEndpoint = viewModel.ApiEndpoint;
                settings.ComplianceThreshold = viewModel.ComplianceThreshold;

                if (settings.Id == 0)
                {
                    _context.Settings.Add(settings);
                }
                else
                {
                    _context.Settings.Update(settings);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index", viewModel);
        }
    }

}

