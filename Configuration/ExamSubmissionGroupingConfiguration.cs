using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineExamProject.Configuration
{
    public class ExamSubmissionGroupingConfiguration : IEntityTypeConfiguration<ExamSubmission>
    {
        public void Configure(EntityTypeBuilder<ExamSubmission> builder)
        {
            builder.ToTable("ExamSubmissions");

            builder.HasKey(es => es.Id);

            builder.Property(es => es.SubmittedAt)
                  .HasDefaultValueSql("GETDATE()");

            builder.HasOne(es => es.User)
                  .WithMany()
                  .HasForeignKey(es => es.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(es => es.ExamSubmissionAnswers)
                  .WithOne(ea => ea.ExamSubmission)
                  .HasForeignKey(ea => ea.ExamSubmissionId)
                  .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
