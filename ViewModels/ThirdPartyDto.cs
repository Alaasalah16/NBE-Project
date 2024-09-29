using System.ComponentModel.DataAnnotations;

namespace NBE_Project.Models
{
    public class ThirdPartyDto
    {
        [MaxLength(20)]

        [Required(ErrorMessage = "*Requird*,Please Enter VendorName")]
        public string? VendorId { get; set; }

       
        [MaxLength(50)]
        public string VendorCategory { get; set; } = "";
        public string Company { get; set; } = "";
        [MaxLength(50)]
        public string ContactTitle { get; set; } = "";
        [Required]
        //[MaxLength(100)]
        public int ContactNumber { get; set; }
        [MaxLength(100)]
        public string Email { get; set; } = "";
        public string ContractName { get; set; } = "";
        public IFormFile? ContractFile { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ContractStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateRange("ContractStartDate", ErrorMessage = "The end date must be later than the start date and within the allowed range.")]
        public DateTime ContractEndDate { get; set; }

        public bool IsProfileCompleted { get; set; }

    }
    }

