using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBE_Project.Models;
using NBE_Project.Services;
using Newtonsoft.Json;


namespace NBE_Project.Controllers
{
    public class VendorHomeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public VendorHomeController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}
