using NBE_Project.Models;
using NBE_Project.ViewModels;

namespace NBE_Project.UserRepositary
{
    public interface IUserRepo
    {
        public Task<List<AppUser>> GetAllUser();
        public Task<AppUser> GetUserByIdAsync(string userId);
        public Task<IEnumerable<string>> GetRolesForUser(string userId);
        public Task<bool> AddRoleToUser(string userId, string roleName);
        public Task<AppUser> RemoveUser(string userId);
        public void RemoveRole();
        public Task<AppUser> AddUser(UserVM model);

        public Task<AppUser> UpdateUser(UserVM model);
    }
}
