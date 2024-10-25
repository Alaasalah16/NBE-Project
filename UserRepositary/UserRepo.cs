//using NBE_Project.GuestRepositary;
using NBE_Project.Models;
using NBE_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NBE_Project.Services;
namespace NBE_Project.UserRepositary
{
    public class UserRepo(UserManager<AppUser> userManager, ApplicationDBContext context) : IUserRepo
    {
        public Task<bool> AddRoleToUser(string userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> AddUser(UserVM model)
        {
            AppUser user = new()
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
              //  Gender = model.Gender,
              //  Phone = model.Phone,
                Email = model.Email,
                PasswordHash = model.Password
            };
            await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, model.RoleName);
            return user;
        }

        public async Task<List<AppUser>> GetAllUser()
        {
            var users = await userManager.Users.ToListAsync();
            return users;
        }

        public Task<IEnumerable<string>> GetRolesForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public void RemoveRole()
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> RemoveUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> UpdateUser(UserVM model)
        {
            throw new NotImplementedException();
        }
    }
}
