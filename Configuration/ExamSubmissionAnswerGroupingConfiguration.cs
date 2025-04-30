using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineExamProject.Configuration
{
    public class ExamSubmissionAnswerGroupingConfiguration : IEntityTypeConfiguration<ExamSubmissionAnswer>
    {
        public void Configure(EntityTypeBuilder<ExamSubmissionAnswer> builder)
        {
            builder.ToTable("ExamSubmissionAnswers");

            builder.HasKey(ea => ea.Id);

            builder.Property(ea => ea.SelectedAnswer)
                  .IsRequired()
                  .HasMaxLength(1);
        }
    }
}
