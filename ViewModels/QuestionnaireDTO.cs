using NBE_Project.Models;

namespace NBE_Project.ViewModels
{
    public class QuestionnaireDTO
    {
        public Question? Question { get; set; }
        public List<QuestionDTO>? Questions { get; set; }
    }

    public class QuestionDTO
    {
        public int Id { get; set; }
        public string? Text { get; set; }
    }

}
