namespace NBE_Project.Services
{
    public interface IMailHelper
    {
        Task SendMail(string Reciver, string Title, string body);
        Task WelcomeEmail(string Name, string Username, string email, string password ,string title);
    //  Task WelcomeEmailAdding( string Username, string email, string password, string title);

    }
}
