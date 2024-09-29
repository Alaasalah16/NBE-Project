namespace NBE_Project.Models
{
    public class Questinare
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public string QuestionOne { get; set; } = "";
        public string QuestionTwo { get; set; } = "";
        public string QuestionThree { get; set; } = "";
        public string QuestionFour { get; set; } = "";
        public string QuestionFive { get; set; } = "";
        public string Description { get; set; } = "";
        /// <summary>
        /// ////try
        /// </summary>
        public int? ThirdPartyId { get; set; }  // Foreign Key
       // public virtual ThirdParty? ThirdParty { get; set; }  // Navigation Property
    }
}