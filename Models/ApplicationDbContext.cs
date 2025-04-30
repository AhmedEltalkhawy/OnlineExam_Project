using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OnlineExamProject.Configuration;
using System.Runtime.InteropServices;

namespace OnlineExamProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ExamSubmission> ExamSubmissions { get; set; }
        public DbSet<ExamSubmissionAnswer> ExamSubmissionAnswers { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Role
            string adminRoleId = "A11";
            string userRoleId = "B2";

            string adminUserId = "A1";

            var Roles = new IdentityRole[]
            {
                new IdentityRole {
                    Id = adminRoleId,
                    Name = "admin",
                    NormalizedName = "ADMIN"
                 } ,
                new IdentityRole {
                    Id = userRoleId,
                    Name = "user",
                    NormalizedName = "USER"
                 } 
               

        };


            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "talkha",
                NormalizedUserName = "TALKHA",
                Email = "ahmedeltalkhawy962@gmail.com",
                NormalizedEmail = "AHMEDELTALKHAWY962@GMAIL.COM",
                EmailConfirmed = true,
                IsAdmin = true ,
                IsDeleted = false ,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            //// Hash Password
            var hasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "7720047");



            //// Seed Data
            builder.Entity<IdentityRole>().HasData(Roles);
            builder.Entity<ApplicationUser>().HasData(adminUser);



            // Assign Admin Role to the User
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });

            builder.ApplyConfigurationsFromAssembly(typeof(ExamGroupingConfiguration).Assembly);



        }

    }
}
