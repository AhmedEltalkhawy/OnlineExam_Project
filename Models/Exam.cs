using System.ComponentModel.DataAnnotations;

namespace OnlineExamProject.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        //public List<Question> Questions { get; set; }

        public List<ExamQuestion> Questions { get; set; } = new List<ExamQuestion>();

        public List<ExamSubmission> ExamSubmissions { get; set; }
    }
}
