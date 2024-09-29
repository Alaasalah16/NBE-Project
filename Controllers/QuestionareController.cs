using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;
using NBE_Project.Services;
using NBE_Project.ViewModels;

namespace NBE_Project.Controllers
{
    public class QuestionareController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QuestionareController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: /Questionare/
        public IActionResult Index()
        {
            var questions = _context.questinares.ToList();
            return View(questions);
        }

        //// GET: /Questionare/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}
        public IActionResult Create(int thirdPartyId)
        {
            var model = new QuestinareDTO
            {
                ThirdPartyId = thirdPartyId // Pass the thirdPartyId to the view model
            };

            return View(model);
        }

        // POST: /Questionare/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuestinareDTO questinareDto)
        {
            if (!ModelState.IsValid)
            {
                return View(questinareDto);
            }

            var questionnaire = new Questinare
            {
                Name = questinareDto.Name,
                QuestionOne = questinareDto.QuestionOne,
                QuestionTwo = questinareDto.QuestionTwo,
                QuestionThree = questinareDto.QuestionThree,
                QuestionFour = questinareDto.QuestionFour,
                QuestionFive = questinareDto.QuestionFive,
                Description = questinareDto.Description,
                ThirdPartyId = questinareDto.ThirdPartyId // Save the ThirdPartyId
            };

            _context.questinares.Add(questionnaire);
            _context.SaveChanges();

            return RedirectToAction("Index", "SecurityAssurance");
        }


        ///Answer
        //public IActionResult Answer(int vendorId)
        //{
        //    var questions = _context.questinares.ToList();
        //    // Ensure vendorId is passed to the view
        //    ViewBag.VendorId = vendorId;
        //    return View(questions);

        //}
        public IActionResult Answer(int vendorId)  
        {
            // Fetch the vendor name based on the vendorId (ThirdPartyId)
            var vendorName = _context.ThirdParties
                .Where(tp => tp.Id == vendorId)
                .Select(tp => tp.VendorName)
                .FirstOrDefault();

            // Fetch the questionnaire answers by vendorId (ThirdPartyId)
            var questions = _context.questinares
                .Where(q => q.ThirdPartyId == vendorId)
                .Select(q => new QuestinareDTO
                {
                    Name = q.Name,
                    QuestionOne = q.QuestionOne,
                    QuestionTwo = q.QuestionTwo,
                    QuestionThree = q.QuestionThree,
                    QuestionFour = q.QuestionFour,
                    QuestionFive = q.QuestionFive,
                    Description = q.Description,
                    ThirdPartyId = q.ThirdPartyId,
                }).ToList();

            // Pass the vendorId and questions to the view
            ViewBag.VendorId = vendorId;
            ViewBag.VendorName = vendorName;  // Add vendor name to ViewBag if needed
            return View(questions);
        }

    }


}


