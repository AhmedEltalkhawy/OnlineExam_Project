using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineExamProject.Repoistary
{
    public class QuestionRepoistary : BaseRepoistary<Question>, IQuestionRepoistary
    {
        public QuestionRepoistary(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<SelectListItem> GetSelectList()
        {
            return GetAll().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Title })
          .OrderBy(c => c.Text)
          .ToList();
        }
    }
}
