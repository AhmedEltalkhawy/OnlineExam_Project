using OnlineExamProject.Const;

namespace OnlineExamProject.Repoistary
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _context;

        public ApplicationDbContext context2 { get; private set; }
        public IExamRepoistary Exams { get; private set; }
        public IQuestionRepoistary Questions { get; private set; }
        public IUserRepoistary Users { get; private set; }

        public IExamQuestionRepoistary ExamQuestions { get; private set; }

        public IExamSubmissionRepoistary ExamSubmissions { get; private set; }





        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
            context2 = context;
            Exams = new ExamRepoistary(_context);
            Questions = new QuestionRepoistary(_context);
            Users = new UserRepoistary(_context);
            ExamQuestions = new ExamQuestionRepoistary(_context);
            ExamSubmissions = new ExamSubmissionRepoistary(_context);

        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
