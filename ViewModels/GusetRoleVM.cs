namespace NBE_Project.ViewModels
{
    public class GusetRoleVM
    {
        public string GuestId { get; set; }
        public string GuestName { get; set; }
        public string RoleName { get; set; }

        public List<string> roleNames = new List<string>();
    }
}
