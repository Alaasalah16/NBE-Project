using NBE_Project.Models;
using NBE_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using NBE_Project.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;

namespace NBE_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMailHelper _mailHelper;
        private readonly SignInManager<AppUser> signInManager;

        public UserController(UserManager<AppUser> userManager, IMailHelper mailHelper,SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _mailHelper = mailHelper;
        }
        public async Task<IActionResult> Index()
        {

            Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            AppUser user = await userManager.FindByIdAsync(userId);
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                // mapping
                AppUser user = new()
                {
                    UserName = registerVM.Username,
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    Address = registerVM.Address,
                    Gender = registerVM.Gender,
                   Phone = registerVM.Phone,
                    Email = registerVM.Email,
                    PasswordHash = registerVM.Password
                };

                IdentityResult result = await userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    // Add user to role
                    //await userManager.AddToRoleAsync(user, "VENDOR");

                    // Send welcome email
                    string emailTitle = "مرحبًا بك في فريق البنك الاهلي";
                    await WelcomeEmail(user.FirstName, user.UserName, user.Email, registerVM.Password, emailTitle);
                   

                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerVM);
        }

        private async Task WelcomeEmail(string name, string username, string email, string password, string title)
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("<html>");
            bodyBuilder.AppendLine("<head>");
            bodyBuilder.AppendLine("<style>");
            bodyBuilder.AppendLine("body { font-family: Arial, sans-serif; }");
            bodyBuilder.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
            bodyBuilder.AppendLine(".message { background-color: #f8f8f8; padding: 20px; border-radius: 5px; }");
            bodyBuilder.AppendLine("h1 { color: #333; }");
            bodyBuilder.AppendLine("</style>");
            bodyBuilder.AppendLine("</head>");
            bodyBuilder.AppendLine("<body>");
            bodyBuilder.AppendLine("<div class='container'>");
            bodyBuilder.AppendLine("<div class='message'>");
            bodyBuilder.AppendLine("<h1>مرحبًا بك في فريق البنك الاهلي!</h1>");
            bodyBuilder.AppendLine($"<p>عزيزي {name},</p>");
            bodyBuilder.AppendLine("<p>نحن سعداء بإعلامك بأن تسجيلك في موقعنا قد تم بنجاح.</p>");
            bodyBuilder.AppendLine("<p>  سيتم التواصل معك قريبا لتاكيد علي بيانتك :</p>");
            bodyBuilder.AppendLine("<p>معلومات الاعتماد الافتراضية الخاصة بك هي:</p>");
            bodyBuilder.AppendLine("<ul>");
            bodyBuilder.AppendLine($"<li><strong>اسم المستخدم:</strong> {username}</li>");
            bodyBuilder.AppendLine($"<li><strong>كلمة المرور الافتراضية:</strong> {password}</li>");
            bodyBuilder.AppendLine("</ul>");
            bodyBuilder.AppendLine("<p>يرجى تغيير كلمة المرور الخاصة بك عند تسجيل الدخول لأول مرة.</p>");
            bodyBuilder.AppendLine("<p>شكرًا لك على الانضمام إلينا!</p>");
            bodyBuilder.AppendLine("<p>فريق البنك الاهلي</p>");
            bodyBuilder.AppendLine("</div>");
            bodyBuilder.AppendLine("</div>");
            bodyBuilder.AppendLine("</body>");
            bodyBuilder.AppendLine("</html>");

            await _mailHelper.SendMail(email, title, bodyBuilder.ToString());
        }
    }
}
