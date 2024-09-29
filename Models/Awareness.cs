using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBE_Project.Models
{
    public class Awareness
    {
        public int? Id { get; set; }
      
         public int? ThirdPartyId { get; set; }
       public virtual ThirdParty? ThirdParty { get; set; }
       // public ThirdParty? VendorName { get; set; }  // Assuming ThirdParty is another model in your project
       // public int? VendorId { get; set; } // Foreign key

      
        [StringLength(500, ErrorMessage = "The message cannot be longer than 500 characters.")]
        public string? Message { get; set; }
        [NotMapped]
        public IFormFile? FileContent { get; set; }  // Stores the content of the uploaded file

        public string? FileName { get; set; }  

    }
}
