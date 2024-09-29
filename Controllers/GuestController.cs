using NBE_Project.GuestRepositary;
using NBE_Project.Models;
using NBE_Project.ViewModels;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace NBE_Project.Controllers
{
    public class GuestController(IGuestRepo GuestRepo, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager) : Controller
    {
        

     //   [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            List<AppUser> customers = await GuestRepo.GetAllUsers();
            return View(customers);
        }
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(string id)
        {
            await GuestRepo.removeUser(id);
            return RedirectToAction("Index", "Guest");
        }
       // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddRole(string id , string name)
        {
            var allRoles = await roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await GuestRepo.GetRolesForUser(id);
            var rolesToAssign = allRoles.Except(userRoles);
            GusetRoleVM model = new GusetRoleVM()
            {
                GuestId = id,
                GuestName = name,
                roleNames = rolesToAssign.ToList() ?? new List<string>(),
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(GusetRoleVM model)
        {
            await GuestRepo.AddRoleToUser(model.GuestId, model.RoleName);

            return RedirectToAction("Index", "Guest");
        }
    }
}
