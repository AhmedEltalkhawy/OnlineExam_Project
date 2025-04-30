namespace OnlineExamProject.Repoistary
{
    public class UserRepoistary : BaseRepoistary<ApplicationUser>, IUserRepoistary
    {
        public UserRepoistary(ApplicationDbContext context) : base(context)
        {
        }
    }
}
