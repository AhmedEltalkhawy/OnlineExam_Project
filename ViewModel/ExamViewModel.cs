using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineExamProject.ViewModel
{
    public class ExamViewModel
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Selected Questions")]
        public List<int>? SelectedQuestions { get; set; } = default!;

        public IEnumerable<SelectListItem> Questions { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
