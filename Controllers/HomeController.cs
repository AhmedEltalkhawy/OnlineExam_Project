using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineExamProject.Models;

namespace OnlineExamProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IUnitOfWork unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        this.unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        //var includes = new string[] { "Questions" };
        //var exams = unitOfWork.Exams.FindAll(e => e.IsDeleted == false,includes);

        var exams = unitOfWork.context2.Exams.
             Include(e => e.ExamSubmissions)
            .Include(e => e.Questions).
            ThenInclude(q => q.Question).Where(e => e.IsDeleted == false)
            .Select(e => new Exam
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description ,
                Questions = e.Questions.Where(q => q.Question.IsDeleted == false).ToList()
            }).ToList();

        return View("Index", exams);
    }

    public IActionResult Search(string Name)
    {
        var includes = new string[] { "Questions" };

        var exams = unitOfWork.Exams.FindAll(c => EF.Functions.Like(c.Title, $"%{Name}%"), includes);
        return View("Index", exams);

    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
