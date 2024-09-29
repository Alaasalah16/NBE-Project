using System.ComponentModel.DataAnnotations;

namespace NBE_Project.Models
{
    public class ThirdParty
    {
        public int Id { get; set; }
        [MaxLength(20)]

        [Required(ErrorMessage = "*Requird*,Please Enter VendorName")]
        public string? VendorName { get; set; }
        [MaxLength(50)]
        public string VendorCategory { get; set; } = "";
        public string Company { get; set; } = "";
        [MaxLength(50)]
        public string ContactTitle { get; set; } = "";
        public int ContactNumber { get; set; }
        [MaxLength(100)]
        public string Email { get; set; } = "";
        public string ContractName { get; set; } = "";
        public IFormFile? ContractFile { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public ICollection<RegulationCompliance> RegulationCompliances { get; set; } = new List<RegulationCompliance>();

        public bool IsProfileCompleted { get; set; }
    

    }
}