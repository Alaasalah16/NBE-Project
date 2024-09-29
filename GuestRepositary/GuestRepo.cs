using NBE_Project.GuestRepositary;
using NBE_Project.Models;
using NBE_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NBE_Project.Services;



namespace NBE_Project.GuestRepositary
{
    public class GuestRepo : IGuestRepo
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDBContext context;

        public GuestRepo(UserManager<AppUser> userManager , ApplicationDBContext _context)
        {
            this.userManager = userManager;
            context = _context;
        }
        public async Task<List<AppUser>> GetAllUsers()
        {
            var allUsers = userManager.Users.ToList();
            return allUsers;
        }
        //public async Task<List<AppUser>> GetAllCustomers()
        //{
        //    var usersInCustomerRole = await userManager.GetUsersInRoleAsync("Vendor");
        //    return usersInCustomerRole.ToList();
        //}
        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }
        public async Task<IEnumerable<string>> GetRolesForUser(string userId)
        {
            var user =await GetUserByIdAsync(userId);
            var userRoles = await userManager.GetRolesAsync(user);
            return userRoles.ToList();

        }
        public async Task<bool> AddRoleToUser(string userId, string roleName)
        {
            var user = await GetUserByIdAsync(userId);
            var result = await userManager.AddToRoleAsync(user, roleName);

            return result.Succeeded;
        }

        public void addUser(AppUser user)
        {
            throw new NotImplementedException();
        }

        

        public void removeRole()
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> removeUser(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return user;
            }

            
            return null;
        }

        public void updateuser(AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
