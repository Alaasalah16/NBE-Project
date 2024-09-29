using Microsoft.AspNetCore.Mvc;
using NBE_Project.Models;
using NBE_Project.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using NBE_Project.Services;

public class QuestionnaireController : Controller
{
    private readonly ApplicationDBContext _context;

    public QuestionnaireController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new QuestionnaireDTO
        {
            Questions = new List<QuestionDTO>()
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuestionnaireDTO model)
    {
        if (ModelState.IsValid)
        {
            foreach (var questionDto in model.Questions)
            {
                var newQuestion = new Question
                {
                    Text = questionDto.Text
                };
                await _context.Questions.AddAsync(newQuestion);
            }

            await _context.SaveChangesAsync();

            // Store question IDs in TempData
            TempData["QuestionIds"] = model.Questions.Select(q => q.Id).ToList();

            return RedirectToAction("DisplayQuestions");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult DisplayQuestions()
    {
        var questions = _context.Questions.ToList();
        return View(questions);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        var question = await _context.Questions.FindAsync(questionId);
        if (question != null)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("DisplayQuestions");
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var questions = await _context.Questions
                                      .Include(q => q.Answers)
                                      .ToListAsync();

        var model = new AnswerDTO
        {
            Questions = questions.Select(q => new AnswerEntry
            {
                Id = q.Id,
                Text = q.Text,
                Answer = q.Answers.FirstOrDefault()?.AnswerText
            }).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Answer()
    {
        var questions = await _context.Questions
                                      .Include(q => q.Answers)
                                      .ToListAsync();

        var model = new AnswerDTO
        {
            Questions = questions.Select(q => new AnswerEntry
            {
                Id = q.Id,
                Text = q.Text,
                Answer = q.Answers.FirstOrDefault()?.AnswerText
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitAnswers(AnswerDTO model)
    {
        if (model == null || model.Questions == null)
        {
            return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }

        if (ModelState.IsValid)
        {
            foreach (var answerEntry in model.Questions)
            {
                if (!string.IsNullOrEmpty(answerEntry.Answer))
                {
                    var answer = new Answer
                    {
                        QuestionId = answerEntry.Id,
                        AnswerText = answerEntry.Answer
                    };

                    await _context.Answers.AddAsync(answer);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("CreateHigh", "SecurityAssurance");
        }

        return View("Answer", model);
    }

    [HttpGet]
    public async Task<IActionResult> DisplayAnswers()
    {
        var answers = await _context.Answers
                                    .Include(a => a.Question)
                                    .ToListAsync();

        var model = answers.GroupBy(a => a.Question)
                           .Select(g => new AnswerDisplayViewModel
                           {
                               Question = g.Key,
                               Answers = g.ToList()
                           }).ToList();

        return View(model);
    }
}
