using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NBE_Project.Models
{
    public class AppUser : IdentityUser
    {
        //[StringLength(100)]
        //[MaxLength(100)]
        //[Required]
        //public string? Name { get; set; }

        //[EmailAddress]
        //[StringLength(256)]
        //public override string? Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Username { get; set; }
        public string Address { get; set; }
       public string Phone { get; set; }
        public string Gender { get; set; }
      // public virtual ICollection<ThirdParty> ThirdParties { get; set; }
    }
}
