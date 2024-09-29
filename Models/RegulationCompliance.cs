using System;
using System.Collections.Generic;

namespace NBE_Project.Models
{
    public class RegulationCompliance
    {
        public int Id { get; set; }
        //public int ThirdPartyId { get; set; }
        public string? RegulationName { get; set; }
        public DateTime ComplianceDate { get; set; }
        public string? Comments { get; set; }
        public List<Clause> Clauses { get; set; } = new List<Clause>();
        public int? ThirdPartyId { get; set; }
        public virtual ThirdParty? ThirdParty { get; set; }

        public string? Feedback { get; set; }
        public string? CompilationStatus { get; set; }


    }

    public class Clause
    {
        public int Id { get; set; }
        public int RegulationComplianceId { get; set; }
        public string? ClauseName { get; set; }
        public string? Requirements { get; set; }
        public string? CompilationStatus { get; set; }
        public string? Comments { get; set; }

        public RegulationCompliance? RegulationCompliance { get; set; }
    }
}