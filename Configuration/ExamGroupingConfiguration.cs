using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineExamProject.Configuration
{
    public class ExamGroupingConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("GETDATE()");

            //builder.HasMany(e => e.Questions)
            //     .WithOne(q => q.Exam)
            //     .HasForeignKey(q => q.ExamId)
            //     .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.ExamSubmissions)
                  .WithOne(es => es.Exam)
                  .HasForeignKey(es => es.ExamId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
