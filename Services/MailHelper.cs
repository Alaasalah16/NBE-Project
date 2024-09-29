using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Text;

namespace NBE_Project.Services
{
    public class MailHelper : IMailHelper
    {
        private readonly MailSettings _mailSettings;
        public MailHelper(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendMail(string receiver, string title, string body)
        {
            try
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Email),
                    Subject = title
                };

                email.To.Add(MailboxAddress.Parse(receiver));
                email.From.Add(new MailboxAddress("Bank", _mailSettings.Email));

                var builder = new BodyBuilder();
                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();

                using (var smtp = new SmtpClient())
                  
                    
                {

                    // Connect to the SMTP server using TLS encryption
                    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.port, SecureSocketOptions.StartTls);

                    // Authenticate with the SMTP server
                    await smtp.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password);

                    // Send the email
                    await smtp.SendAsync(email);

                    // Disconnect from the SMTP server
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
            }
        }

        public async Task WelcomeEmail(string Name, string Username, string email, string password,string title)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("<html>");
            bodyBuilder.AppendLine("<head>");
            bodyBuilder.AppendLine("<style>");
            bodyBuilder.AppendLine("body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f5f5f5; color: #333; margin: 0; padding: 0; }");
            bodyBuilder.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
            bodyBuilder.AppendLine("h1 { color: #007bff; text-align: center; }");
            bodyBuilder.AppendLine("p { margin-bottom: 15px; line-height: 1.6; }");
            bodyBuilder.AppendLine("ul { margin-bottom: 20px; }");
            bodyBuilder.AppendLine("li { margin-bottom: 5px; }");
            bodyBuilder.AppendLine(".message { background-color: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); }");
            bodyBuilder.AppendLine("</style>");
            bodyBuilder.AppendLine("</head>");
            bodyBuilder.AppendLine("<body>");
            bodyBuilder.AppendLine("<div class='container'>");
            bodyBuilder.AppendLine("<div class='message'>");
            bodyBuilder.AppendLine("<h1>مرحبًا بك في فريق البنك الاهلي !</h1>");
            bodyBuilder.AppendLine($"<p>عزيزي {Name},</p>");
            bodyBuilder.AppendLine("<p>نحن سعداء بإعلامك بأن تسجيلك  في موقع البنك الاهلي  قد تم بنجاح.</p>");
            bodyBuilder.AppendLine("<p>معلومات الاعتماد الافتراضية الخاصة بك هي:</p>");
            bodyBuilder.AppendLine("<ul>");
            bodyBuilder.AppendLine($"<li><strong>اسم المستخدم:</strong> {Username}</li>");
            bodyBuilder.AppendLine($"<li><strong>كلمة المرور الافتراضية:</strong> {password}</li>");
            bodyBuilder.AppendLine("</ul>");
            bodyBuilder.AppendLine("<p>نوصي بشدة بتغيير كلمة المرور الافتراضية فور تسجيل الدخول الأول الخاص بك. يمكنك القيام بذلك عن طريق التوجه إلى إعدادات الحساب الخاص بك.</p>");
            bodyBuilder.AppendLine("<p>نأمل أن تكون تجربتك معنا مثمرة وممتعة. إذا كانت لديك أي استفسارات أو اقتراحات، فلا تتردد في التواصل معنا.</p>");
            bodyBuilder.AppendLine("<p>مرة أخرى، نرحب بك في فريقنا ونتمنى لك كل التوفيق في مهمتك.</p>");
            bodyBuilder.AppendLine("<p>أطيب التحيات،</p>");
            bodyBuilder.AppendLine("<p>فريق البنك الاهلي </p>");
            bodyBuilder.AppendLine("</div>"); // Closing the div with class 'message'
            bodyBuilder.AppendLine("</div>"); // Closing the div with class 'container'
            bodyBuilder.AppendLine("</body>");
            bodyBuilder.AppendLine("</html>");
            await SendMail(email,title, bodyBuilder.ToString());
        }

     
    }
}
