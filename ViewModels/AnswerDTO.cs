namespace NBE_Project.ViewModels
{
    public class AnswerDTO
    {
        public List<AnswerEntry>? Questions { get; set; }
        public List<AnswerEntry>? Answers { get; set; }
    }

    public class QuestionEntry
    {
        public int Id { get; set; }
        public string? Text { get; set; }
    }

    public class AnswerEntry
    {
        public int Id { get; set; } 
        public string? Text { get; set; } 
        public string? Answer { get; set; }
        public int QuestionId { get; set; } 
       
    }
}

