using System.ComponentModel.DataAnnotations;

namespace NBE_Project.ViewModels
{
    public class RegisterVM
    {


        //public int Id { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "username must be greater than 5 letter")]
        public string Username { get; set; }
        [Required]

        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public string Phone { get; set; }
        [Required]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }

        [Required]
        public string Address { get; set; }
        [Required(ErrorMessage = "*")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must match Confirm Password")]
        public string ConfirmPassword { get; set; }
        //[Required]
        //[Display(Name = "Name")]
        //public string Name { get; set; }

        //[Required]
        //[Display(Name = "Username")]
        //public string Username { get; set; }

        //[Required]
        //[Display(Name = "Company")]
        //public string Company { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm Password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
    }
}
