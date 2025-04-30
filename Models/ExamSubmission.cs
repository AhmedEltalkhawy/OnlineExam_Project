namespace OnlineExamProject.Models
{
    public class ExamSubmission
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } 

        [Required]
        public int ExamId { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public float Score { get; set; }

        public bool Passed { get; set; }
        public int CorrectAnswer  { get; set; }
        public int WrongAnswer { get; set; }
        public int TotalQuestion => CorrectAnswer + WrongAnswer;







        public Exam? Exam { get; set; }
        public ApplicationUser? User { get; set; } 
        public List<ExamSubmissionAnswer>? ExamSubmissionAnswers { get; set; }
    }
}
