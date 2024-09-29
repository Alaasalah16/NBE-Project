using Microsoft.AspNetCore.Mvc;
using NBE_Project.Services;
using NBE_Project.ViewModels;

namespace NBE_Project.Controllers
{
    public class MailingController : Controller
    {
        private readonly IMailHelper _mailHelper;

        public MailingController(IMailHelper mailHelper)
        {
            _mailHelper = mailHelper;
        }
        //[HttpPost("send")]
        //public async <IActionResult> SendMail(EmailRequestDto mailRequeastDto)
        //{
        //    await _mailHelper.SendMailAsync(mailRequeastDto.receiver, mailRequeastDto.title, mailRequeastDto.body);
        //    return View();
        //}
    }
}
