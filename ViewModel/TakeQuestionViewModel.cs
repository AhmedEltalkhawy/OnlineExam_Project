namespace OnlineExamProject.ViewModel
{
    public class TakeQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string? Title { get; set; }
        public string? ChoiceA { get; set; }
        public string? ChoiceB { get; set; }
        public string? ChoiceC { get; set; }
        public string? ChoiceD { get; set; }

        [Required(ErrorMessage = "Please select an answer")]
        [RegularExpression("A|B|C|D")]
        public string SelectedAnswer { get; set; }
    }
}
