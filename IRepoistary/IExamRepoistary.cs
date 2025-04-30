using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineExamProject.IRepoistary
{
    public interface IExamRepoistary : IBaseRepoistary<Exam>
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
