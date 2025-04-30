using Microsoft.AspNetCore.Mvc;
using OnlineExamProject.Models;
using OnlineExamProject.ViewModel;

namespace OnlineExamProject.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public QuestionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var questions = unitOfWork.Questions.GetAll();

            return View("Index" , questions);
        }

        public IActionResult ActiveQuestions()
        {
            var questions = unitOfWork.Questions.FindAll(e => e.IsDeleted == false);
           
            return View("Index", questions);
        }

        public IActionResult DeletedQuestions()
        {
            var questions = unitOfWork.Questions.FindAll(e => e.IsDeleted == true);

            return View("Index", questions);
        }

        public IActionResult Search(string Name)
        {

            var questions = unitOfWork.Questions.FindAll(c => EF.Functions.Like(c.Title, $"%{Name}%"));
            return View("Index", questions);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var includes = new string[] {  };
            //var question = await unitOfWork.Questions.FindAsync(e => e.Id == id, includes);

            var question = await unitOfWork.context2.Questions.Include(e => e.Exams).ThenInclude(q => q.Exam).FirstAsync(e => e.Id == id);

            if (question == null)
            {
                return NotFound();
            }


            return View("Details", question);
        }

        public IActionResult Create()
        {
            QuestionViewModel model = new QuestionViewModel
            {
                Exams = unitOfWork.Exams.GetSelectList()
            };
            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Exams = unitOfWork.Exams.GetSelectList();
                return View("Create", model);
            }
            var question= new Question
            {
                Title = model.Title,
                IsDeleted = false,
                ChoiceA = model.ChoiceA , 
                ChoiceB = model.ChoiceB , 
                ChoiceC = model.ChoiceC ,
                ChoiceD = model.ChoiceD , 
                CorrectAnswer = model.CorrectAnswer , 
                
            };

            if(model.SelectedExams is not null)
            {
                question.Exams = model.SelectedExams.Select(d => new ExamQuestion { ExamId = d }).ToList();

            }
            unitOfWork.Questions.Add(question);
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var includes = new string[] { "Exams" };
            var question = unitOfWork.Questions.Find(e => e.Id == id, includes);

            if (question is null)
                return NotFound();

            QuestionViewModel model = new QuestionViewModel
            {
                Id = question.Id,
                Exams = unitOfWork.Exams.GetSelectList(),
                Title = question.Title,
                ChoiceA = question.ChoiceA ,
                ChoiceB = question.ChoiceB ,
                ChoiceC = question.ChoiceC ,
                ChoiceD = question.ChoiceD ,
                CorrectAnswer = question.CorrectAnswer ,
                SelectedExams= question.Exams.Select(e => e.QuestionId).ToList()


            };
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdit(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Exams = unitOfWork.Exams.GetSelectList();
                return View("Edit", model);
            }

            var includes = new string[] { "Exams" };
            var question = unitOfWork.Questions.Find(e => e.Id == model.Id, includes);

            if (question is null)
                return NotFound();

            question.Title = model.Title;
            question.ChoiceA = model.ChoiceA;
            question.ChoiceB = model.ChoiceB;
            question.ChoiceC = model.ChoiceC;
            question.ChoiceD = model.ChoiceD;
            question.CorrectAnswer = model.CorrectAnswer;
            if (model.SelectedExams is not null)
            {
                question.Exams = model.SelectedExams.Select(d => new ExamQuestion { ExamId = d }).ToList();

            }
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var question = unitOfWork.Questions.Find(e => e.Id == id);
            question.IsDeleted = true;
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult UndoDelete(int id)
        {
            var question = unitOfWork.Questions.Find(e => e.Id == id);
            question.IsDeleted = false;
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveQuestionFromExam(int examId, int questionId)
        {
            var examQuestion = await unitOfWork.ExamQuestions.FindAsync(eq => eq.ExamId == examId && eq.QuestionId == questionId);

            if (examQuestion != null)
            {
                unitOfWork.ExamQuestions.Delete(examQuestion);
                unitOfWork.Complete();
            }

            return RedirectToAction("Details", new { id = examId });
        }



    }
}
