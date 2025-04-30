namespace OnlineExamProject.ViewModel
{
    public class TakeExamViewModel
    {
        public int ExamId { get; set; }
        public string? ExamTitle { get; set; }
        public string? Description { get; set; }
        public List<TakeQuestionViewModel> Questions { get; set; } = new();
    }
}
