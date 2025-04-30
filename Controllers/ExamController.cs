using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamProject.Const;
using OnlineExamProject.Models;
using OnlineExamProject.ViewModel;

namespace OnlineExamProject.Controllers
{
    public class ExamController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> usermanager;


        public ExamController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> usermanager)
        {
            this.unitOfWork = unitOfWork;
            this.usermanager = usermanager;
        }

        public IActionResult Index()
        {
            var includes = new string[] { "Questions" };
            var exams = unitOfWork.Exams.GetAll(includes);
            return View("Index" , exams);
        }

        public IActionResult ActiveExams()
        {
            var includes = new string[] { "Questions" };
            var exams = unitOfWork.Exams.FindAll( e => e.IsDeleted == false, includes)
                .OrderByDescending(o => o.CreatedAt).ToList(); ;

            return View("Index", exams);
        }

        public IActionResult DeletedExams()
        {
            var includes = new string[] { "Questions" };
            var exams = unitOfWork.Exams.FindAll(e => e.IsDeleted == true, includes)
                .OrderByDescending(o => o.CreatedAt).ToList(); ;

            return View("Index", exams);
        }

        public IActionResult Search(string Name)
        {
            var includes = new string[] { "Questions" };

            var exams = unitOfWork.Exams.FindAll(c => EF.Functions.Like(c.Title, $"%{Name}%"), includes);
            return View("Index", exams);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) 
            {
                return NotFound();
            }

            //var includes = new string[] { "Questions" };
            //var exam = await unitOfWork.Exams.FindAsync(e => e.Id == id , includes);
            var exam = await unitOfWork.context2.Exams.Include(e => e.Questions).ThenInclude(q => q.Question).FirstAsync(e => e.Id == id);
            if (exam == null)
            { 
                return NotFound(); 
            }


            return View("Details", exam);
        }

        public IActionResult Create()
        {
            ExamViewModel model = new ExamViewModel
            {
                Questions = unitOfWork.Questions.GetSelectList()
                
            };
            return View("Create" , model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(ExamViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Questions = unitOfWork.Questions.GetSelectList();
                return View("Create", model);
            }
            var exam = new Exam
            {
                Title = model.Title , 
                Description = model.Description ,
                IsDeleted = false 
            };

            if(model.SelectedQuestions is not null)
            {
                exam.Questions = model.SelectedQuestions.Select(d => new ExamQuestion { QuestionId = d }).ToList();

            }

            unitOfWork.Exams.Add(exam);
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var includes = new string[] { "Questions" };
            var exam = unitOfWork.Exams.Find(e => e.Id == id , includes);

            if (exam is null)
                return NotFound();

            ExamViewModel model = new ExamViewModel
            {
                Id = exam.Id ,
                Questions = unitOfWork.Questions.GetSelectList(),
                Description = exam.Description,
                Title = exam.Title,
                SelectedQuestions = exam.Questions.Select(e => e.QuestionId).ToList()


            };
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEdit(ExamViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Questions = unitOfWork.Questions.GetSelectList();
                return View("Edit", model);
            }

            var includes = new string[] { "Questions" };
            var exam = unitOfWork.Exams.Find(e => e.Id == model.Id , includes) ;

            if (exam is null)
                return NotFound();

            exam.Title = model.Title;
            exam.Description = model.Description;
            if (model.SelectedQuestions is not null)
            {
                exam.Questions = model.SelectedQuestions.Select(d => new ExamQuestion { QuestionId = d }).ToList();

            }
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var exam = unitOfWork.Exams.Find(e => e.Id == id);
            exam.IsDeleted = true;
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult UndoDelete(int id)
        {
            var exam = unitOfWork.Exams.Find(e => e.Id == id);
            exam.IsDeleted = false;
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AJAxUndoDelete(int id)
        {
            var exam = unitOfWork.Exams.Find(e => e.Id == id);
            if (exam == null)
            {
                return Json(new { success = false, message = "Exam not found." });
            }

            exam.IsDeleted = false;
            unitOfWork.Complete();

            return Json(new { success = true, message = "Undo delete successful." });
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


        [HttpGet]
        public async Task<IActionResult> TakeExam(int id)
        {
            var user = await usermanager.GetUserAsync(User);

            var exams = unitOfWork.context2.Exams.Include(e => e.Questions).
              ThenInclude(q => q.Question).Where(e => e.IsDeleted == false)
              .Select(e => new Exam
              {
                  Id = e.Id,
                  Title = e.Title,
                  Description = e.Description,
                  Questions = e.Questions.Where(q => q.Question.IsDeleted == false).ToList()
              }).ToList();

            var exam = exams.FirstOrDefault(e => e.Id == id);


            if (exam == null)
                return NotFound();

            var submission = unitOfWork.ExamSubmissions.Find(es => es.ExamId == id && es.UserId
            == user.Id);

            if(submission is not null)
            {
                return RedirectToAction("Result", new { ExamId = exam.Id });

            }


           

            var model = new TakeExamViewModel
            {
                ExamId = exam.Id,
                ExamTitle = exam.Title,
                Description = exam.Description ?? "_",
                Questions = exam.Questions.Select(eq => new TakeQuestionViewModel
                {
                    QuestionId = eq.Question.Id,
                    Title = eq.Question.Title,
                    ChoiceA = eq.Question.ChoiceA,
                    ChoiceB = eq.Question.ChoiceB,
                    ChoiceC = eq.Question.ChoiceC,
                    ChoiceD = eq.Question.ChoiceD
                }).ToList()
            };

            return View("TakeExam", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTakeExam(TakeExamViewModel model)
        {
            if (!ModelState.IsValid)
                return View("TakeExam", model);

            var user = await usermanager.GetUserAsync(User);

            var exam = await unitOfWork.context2.Exams
                 .Include(e => e.Questions)
                 .ThenInclude(q => q.Question)
                 .FirstAsync(e => e.Id == model.ExamId  );

         

            if (exam == null)
                return NotFound();

            int correct = 0;
            var submission = new ExamSubmission
            {
                UserId = user.Id,
                ExamId = exam.Id,
                SubmittedAt = DateTime.Now,
                CorrectAnswer = 0 ,
                WrongAnswer = 0 ,
                ExamSubmissionAnswers = new List<ExamSubmissionAnswer>()
            };

            foreach (var q in model.Questions)
            {
                var actualQuestion = exam.Questions.FirstOrDefault(eq => eq.Question.Id == q.QuestionId)?.Question;
                if (actualQuestion == null) continue;

                bool isCorrect = q.SelectedAnswer == actualQuestion.CorrectAnswer;
                if (isCorrect)
                {
                    correct++;
                    ++submission.CorrectAnswer;
                }
                else
                {
                    ++submission.WrongAnswer;
                }

                    submission.ExamSubmissionAnswers.Add(new ExamSubmissionAnswer
                    {
                        QuestionId = q.QuestionId,
                        SelectedAnswer = q.SelectedAnswer,
                        IsCorrect = isCorrect
                    });
            }

            submission.Score = (float) correct / model.Questions.Count * 100;
            submission.Passed = submission.Score >= 60;

            unitOfWork.ExamSubmissions.Add(submission);
            unitOfWork.Complete();

            return RedirectToAction("Result", new { ExamId = exam.Id });
        }

        public async Task<IActionResult> Result(int ExamId)
        {
            var user = await usermanager.GetUserAsync(User);


            var submission = await unitOfWork.context2.ExamSubmissions.
                Include(s => s.Exam)
                .Include(s => s.ExamSubmissionAnswers)
                .ThenInclude(s => s.Question)
                .FirstOrDefaultAsync(s => s.ExamId == ExamId && s.UserId == user.Id);
            if (submission == null)
                return View("ExamNotToken");

            return View("Result" ,submission);
        }


        /*
         * take Exam question by question using AJAX CALL
         */

        [HttpGet]
        public async Task<IActionResult> StartExam(int examId)
        {
            var user = await usermanager.GetUserAsync(User);

            var exams = unitOfWork.context2.Exams.Include(e => e.Questions).
              ThenInclude(q => q.Question).Where(e => e.IsDeleted == false)
              .Select(e => new Exam
              {
                  Id = e.Id,
                  Title = e.Title,
                  Description = e.Description,
                  Questions = e.Questions.Where(q => q.Question.IsDeleted == false).ToList()
              }).ToList();

            var exam = exams.FirstOrDefault(e => e.Id == examId);


            if (exam == null)
                return NotFound();

            var submission = unitOfWork.ExamSubmissions.Find(es => es.ExamId == examId && es.UserId
            == user.Id);

            if (submission is not null)
            {
                return RedirectToAction("Result", new { ExamId = exam.Id });

            }

            return View("TakeExamQuestionByQuestion", examId);
        }


        [HttpGet]
        public async Task<IActionResult> GetQuestion(int examId, int index)
        {
            var exams = unitOfWork.context2.Exams.Include(e => e.Questions).
              ThenInclude(q => q.Question).Where(e => e.IsDeleted == false)
              .Select(e => new Exam
              {
                  Id = e.Id,
                  Title = e.Title,
                  Description = e.Description,
                  Questions = e.Questions.Where(q => q.Question.IsDeleted == false).ToList()
              }).ToList();

            var exam = exams.FirstOrDefault(e => e.Id == examId);


            if (exam == null)
                return NotFound();

            if (exam == null || index < 0 || index >= exam.Questions.Count)
                return NotFound();

            var question = exam.Questions.OrderBy(q => q.QuestionId).ToList()[index].Question;

            var model = new TakeExamStepViewModel
            {
                ExamId = exam.Id,
                QuestionIndex = index,
                TotalQuestions = exam.Questions.Count,
                CurrentQuestion = new TakeQuestionViewModel
                {
                    QuestionId = question.Id,
                    Title = question.Title,
                    ChoiceA = question.ChoiceA,
                    ChoiceB = question.ChoiceB,
                    ChoiceC = question.ChoiceC,
                    ChoiceD = question.ChoiceD
                }
            };

            return PartialView("_QuestionPartial", model);
        }


        [HttpPost]
        public async Task<IActionResult> SubmitExam([FromBody] SubmitExamRequest model)
        {
            if(!ModelState.IsValid)
            {
                return View("TakeExamQuestionByQuestion", model.ExamId);

            }
            var user = await usermanager.GetUserAsync(User);

            var exam = await unitOfWork.context2.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Question)
                .FirstAsync(e => e.Id == model.ExamId);

            if (exam == null)
                return NotFound();

            int correct = 0;
            var submission = new ExamSubmission
            {
                ExamId = model.ExamId,
                UserId = user.Id,
                SubmittedAt = DateTime.Now,
                CorrectAnswer = 0,
                WrongAnswer = exam.Questions.Count,
                ExamSubmissionAnswers = new List<ExamSubmissionAnswer>()
            };



            foreach (var answer in model.Answers)
            {
                bool isCorrect;
                var question = exam.Questions.FirstOrDefault(q => q.QuestionId == answer.QuestionId)?.Question;
                if (question == null) continue;

                 isCorrect = question.CorrectAnswer == answer.SelectedAnswer;
                if (isCorrect)
                {
                    correct++;
                    ++submission.CorrectAnswer;
                    --submission.WrongAnswer;
                }
                

                submission.ExamSubmissionAnswers.Add(new ExamSubmissionAnswer
                {
                    QuestionId = question.Id,
                    SelectedAnswer = answer.SelectedAnswer,
                    IsCorrect = isCorrect
                });
            }

            submission.Score = (float)correct / exam.Questions.Count * 100;
            submission.Passed = submission.Score >= 60;

            unitOfWork.ExamSubmissions.Add(submission);
            unitOfWork.Complete();

            //return RedirectToAction("Result", new { ExamId = exam.Id });
            return Ok(new { ExamId = exam.Id});

        }

        public IActionResult SubmitSuccess()
        {
            return View("SubmitSuccess");
        }


    }
}
