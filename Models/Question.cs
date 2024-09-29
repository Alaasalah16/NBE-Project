namespace NBE_Project.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Answer { get; set; }
        public int QuestionnaireId { get; set; }
        public ICollection<Answer>? Answers { get; set; }
     

    }

    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string? AnswerText { get; set; }
        public int QuestionnaireId { get; set; }
        public Question? Question { get; set; }

    }
}
