using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineExamProject.IRepoistary
{
    public interface IQuestionRepoistary :  IBaseRepoistary<Question>
    {
        IEnumerable<SelectListItem> GetSelectList();
        //IEnumerable<SelectListItem> GetQuestionList();
    }
}
