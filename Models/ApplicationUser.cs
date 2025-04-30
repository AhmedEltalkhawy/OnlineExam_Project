
namespace OnlineExamProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
