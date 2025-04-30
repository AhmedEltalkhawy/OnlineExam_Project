
namespace OnlineExamProject.Models
{
    public class Question
    {
        public int Id { get; set; }

        //[Required]
        //public int ExamId { get; set; }

        [Required]
        public string Title { get; set; }
        public bool IsDeleted { get; set; } = false;


        [Required]
        public string ChoiceA { get; set; }

        [Required]
        public string ChoiceB { get; set; }

        [Required]
        public string ChoiceC { get; set; }

        [Required]
        public string ChoiceD { get; set; }

        [Required]
        [RegularExpression("A|B|C|D")]
        public string CorrectAnswer { get; set; }

        // Navigation Properties
        //public Exam Exam { get; set; }

        public List<ExamQuestion> Exams { get; set; } = new List<ExamQuestion>();

        public List<ExamSubmissionAnswer> ExamSubmissionAnswers { get; set; }
    }
}
