using NBE_Project.Models;


namespace NBE_Project.GuestRepositary
{
    public interface IGuestRepo
    {
        public  Task<List<AppUser>> GetAllUsers();
        //public  Task<List<AppUser>> GetAllCustomers();
        public Task<AppUser> GetUserByIdAsync(string userId);
        public Task<IEnumerable<string>> GetRolesForUser(string userId);
        public  Task<bool> AddRoleToUser(string userId, string roleName);
        public Task<AppUser> removeUser(string userId);
        public void removeRole();
        public void addUser(AppUser user);
        
        public void updateuser(AppUser user);
    }
}
