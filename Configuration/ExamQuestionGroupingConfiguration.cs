using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineExamProject.Configuration
{
    public class ExamQuestionGroupingConfiguration : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.HasKey(e => new { e.ExamId, e.QuestionId });
            builder.ToTable("ExamQuestion");
        }
    }
}
