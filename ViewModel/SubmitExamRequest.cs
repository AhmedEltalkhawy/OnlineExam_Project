namespace OnlineExamProject.ViewModel
{
    public class SubmitExamRequest
    {
        public int ExamId { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
}
