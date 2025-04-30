namespace OnlineExamProject.Models
{
    public class ExamSubmissionAnswer
    {
        public int Id { get; set; }

        [Required]
        public int ExamSubmissionId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        [RegularExpression("A|B|C|D")]
        public string SelectedAnswer { get; set; }

        public bool IsCorrect { get; set; }

        // Navigation Properties
        public ExamSubmission ExamSubmission { get; set; }
        public Question Question { get; set; }
    }
}
