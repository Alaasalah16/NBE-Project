using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectListItem

namespace NBE_Project.ViewModels
{
    public class RegulationComplianceVM
    {
        public int Id { get; set; }
      //  public int ThirdPartyId { get; set; }
        public string? RegulationName { get; set; }
        public DateTime ComplianceDate { get; set; }
        public string? Comments { get; set; }
        public List<ClauseVM> Clauses { get; set; } = new List<ClauseVM>();


        public int? ThirdPartyId { get; set; }
        public string? VendorName { get; set; }
        public List<SelectListItem> ThirdPartyList { get; set; } = new List<SelectListItem>();
        public string? Feedback { get; set; }
        public string? CompilationStatus { get; set; }

    }

    public class ClauseVM
    {
        public int Id { get; set; }
        public string? ClauseName { get; set; }
        public string? Requirements { get; set; }
        public string? CompilationStatus { get; set; }
        public string? Comments { get; set; }
    }
}