using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;
using NBE_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Text;
using NBE_Project.Services;


namespace NBE_Project.Controllers
{

        public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMailHelper _mailHelper;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, IMailHelper mailHelper, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
           _mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginVM.username, loginVM.Password, loginVM.IsPresistent, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(loginVM.username);

                    // Sending a welcome email after successful login
                    await WelcomeEmail(user.FirstName, user.UserName, user.Email, user.PasswordHash, "Welcome to ElBank Elhaly");

                    if (user != null)
                   
                    
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else if (roles.Contains("Vendor"))
                        {
                            return RedirectToAction("Index", "VendorHome");
                        }
                        // Add a default redirect if no specific role is matched
                        //return RedirectToAction("Login", "Account");
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginVM);
        }

    

        [HttpGet]
            public async Task<IActionResult> Logout()
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Index", "thirdParty");
            }


        public async Task<AppUser> FindByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
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
            bodyBuilder.AppendLine("<h1>مرحبًا بك في فريق البنك الاهلي </h1>");
            bodyBuilder.AppendLine($"<p>عزيزي {name},</p>");
            bodyBuilder.AppendLine("<p>نحن سعداء بإعلامك بأن تسجيلك في موقعنا  قد تم بنجاح.</p>");
            bodyBuilder.AppendLine("<p>معلومات الاعتماد الافتراضية الخاصة بك هي:</p>");
            bodyBuilder.AppendLine("<ul>");
            bodyBuilder.AppendLine($"<li><strong>اسم المستخدم:</strong> {username}</li>");
            bodyBuilder.AppendLine($"<li><strong>كلمة المرور الافتراضية:</strong> {password}</li>");
            bodyBuilder.AppendLine("</ul>");
            bodyBuilder.AppendLine("<p>يرجى تغيير كلمة المرور الخاصة بك عند تسجيل الدخول لأول مرة.</p>");
            bodyBuilder.AppendLine("<p>شكرًا لك على الانضمام إلينا!</p>");
            bodyBuilder.AppendLine("<p>فريق البنك الاهلي </p>");
            bodyBuilder.AppendLine("</div>");
            bodyBuilder.AppendLine("</div>");
            bodyBuilder.AppendLine("</body>");
            bodyBuilder.AppendLine("</html>");

            await _mailHelper.SendMail(email, title, bodyBuilder.ToString());
        }
    }
    }
























//    private readonly UserManager<AppUser> userManager;
//    private readonly SignInManager<AppUser> signInManager;

//    public AccountController(UserManager<AppUser> userManager,
//        SignInManager<AppUser> signInManager)
//    {
//        this.userManager = userManager;
//        this.signInManager = signInManager;
//    }

//    public IActionResult Index()
//    {
//        return View();
//    }
//    [HttpGet]
//    public IActionResult Login()
//    {
//        return View();
//    }
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Login(LoginVM loginVM)
//    {
//        if (ModelState.IsValid)
//        {
//            AppUser user = await userManager.FindByNameAsync(loginVM.username);
//            if (user != null)
//            {
//                bool found = await userManager.CheckPasswordAsync(user, loginVM.Password);
//                if (found)
//                {
//                    await signInManager.SignInAsync(user, isPersistent: loginVM.IsPresistent);

//                    //claims
//                    //  await signInManager.SignInWithClaimsAsync(user, loginVM.IsPresistent);
//                    if (User.IsInRole("Admin"))
//                    {
//                        return RedirectToAction("Index", "Home");

//                    }
//                    if (User.IsInRole("Vendor"))
//                    {
//                        return RedirectToAction("Index", "thirdparty");

//                    }// create cookie

//                }
//            }

//            ModelState.AddModelError("", "Invalid username or password");
//        }
//        return View(loginVM);
//    }