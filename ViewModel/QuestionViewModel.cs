using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineExamProject.ViewModel
{
    public class QuestionViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string ChoiceA { get; set; }

      
        public string ChoiceB { get; set; }

        public string ChoiceC { get; set; }

        public string ChoiceD { get; set; }

        [RegularExpression("A|B|C|D")]
        public string CorrectAnswer { get; set; }

        [Display(Name = "Exams")]
        public List<int>? SelectedExams { get; set; } = default!;
        public IEnumerable<SelectListItem> Exams { get; set; } = Enumerable.Empty<SelectListItem>();


    }
}
