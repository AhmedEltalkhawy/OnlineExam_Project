namespace OnlineExamProject.Models
{
    public class ExamQuestion
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; } = default!;
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = default!;

    }
}
