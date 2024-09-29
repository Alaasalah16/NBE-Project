using NBE_Project.Models;

namespace NBE_Project.ViewModels
{
    public class AnswerDisplayViewModel
    {
        public Question? Question { get; set; }
        public List<Answer>? Answers { get; set; }
    }
}
