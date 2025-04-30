namespace OnlineExamProject.IRepoistary
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext context2 { get; }
        IExamRepoistary Exams { get; }
        IQuestionRepoistary Questions { get; }
        IUserRepoistary Users { get; }
        IExamQuestionRepoistary ExamQuestions { get; }
        IExamSubmissionRepoistary ExamSubmissions { get; }

        int Complete();
    }
}
