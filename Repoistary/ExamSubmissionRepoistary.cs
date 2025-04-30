namespace OnlineExamProject.Repoistary
{
    public class ExamSubmissionRepoistary : BaseRepoistary<ExamSubmission>, IExamSubmissionRepoistary
    {
        public ExamSubmissionRepoistary(ApplicationDbContext context) : base(context)
        {
        }
    }
}
