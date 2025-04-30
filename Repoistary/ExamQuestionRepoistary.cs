namespace OnlineExamProject.Repoistary
{
    public class ExamQuestionRepoistary : BaseRepoistary<ExamQuestion>, IExamQuestionRepoistary
    {
        public ExamQuestionRepoistary(ApplicationDbContext context) : base(context)
        {
        }
    }
}
