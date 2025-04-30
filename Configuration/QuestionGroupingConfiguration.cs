using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineExamProject.Configuration
{
    public class QuestionGroupingConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Title)
                  .IsRequired();

            builder.Property(q => q.ChoiceA)
                  .IsRequired();

            builder.Property(q => q.ChoiceB)
                  .IsRequired();

            builder.Property(q => q.ChoiceC)
                  .IsRequired();

            builder.Property(q => q.ChoiceD)
                  .IsRequired();

            builder.Property(q => q.CorrectAnswer)
                  .IsRequired()
                  .HasMaxLength(1);

            builder.HasMany(q => q.ExamSubmissionAnswers)
                 .WithOne(ea => ea.Question)
                 .HasForeignKey(ea => ea.QuestionId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
