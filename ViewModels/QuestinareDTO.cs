using Microsoft.AspNetCore.Mvc.Rendering;

namespace NBE_Project.ViewModels
{
    public class QuestinareDTO
    {
        public string Name { get; set; } = "";

        public string QuestionOne { get; set; } = "";
        public string QuestionTwo { get; set; } = "";
        public string QuestionThree { get; set; } = "";
        public string QuestionFour { get; set; } = "";
        public string QuestionFive { get; set; } = "";
        public string Description { get; set; } = "";
        public int? ThirdPartyId { get; set; }

        //public string? VendorName { get; set; }
        //public List<SelectListItem> ThirdPartyList { get; set; } = new List<SelectListItem>();
    }
}
