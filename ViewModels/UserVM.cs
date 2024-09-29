using System.ComponentModel.DataAnnotations;
namespace NBE_Project.ViewModels
{
    public class UserVM
    {
        [Required]
        [MinLength(5, ErrorMessage = "username must be greater than 5 letter")]
        public string Username { get; set; }
        [Required]

        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public string Phone { get; set; }
       // [Required]
       // [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
       // public string Gender { get; set; }

     //   [Required]
        public string Address { get; set; }= "Cairo";
        [Required(ErrorMessage = "*")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must match Confirm Password")]
        public string ConfirmPassword { get; set; }
       // [Required]
       public string RoleName { get; set; } 
    }
}
