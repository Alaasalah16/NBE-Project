using System.ComponentModel.DataAnnotations;

namespace NBE_Project.ViewModels
{
    public class LoginVM
    {

        public int Id { get; set; }

        [Required]
        public string username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsPresistent { get; set; } = true;
        //[Required]
        //[Display(Name = "Username")]
        //public string Username { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[Display(Name = "Remember me")]
        //public bool RememberMe { get; set; }
        //public string UserType { get; set; }
    }
}
