namespace OnlineExamProject.ViewModel
{
    public class TakeExamStepViewModel
    {
        public int ExamId { get; set; }
        public int QuestionIndex { get; set; } 
        public int TotalQuestions { get; set; }
        public TakeQuestionViewModel CurrentQuestion { get; set; }
    }
}
