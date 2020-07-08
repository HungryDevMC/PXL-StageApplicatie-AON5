using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stage_API.Domain;

namespace Stage_API.Data.Configuration
{
    public class ReviewerInternshipsConfiguration : IEntityTypeConfiguration<ReviewerInternships>
    {
        public void Configure(EntityTypeBuilder<ReviewerInternships> builder)
        {
            builder.ToTable("Reviewer_Internships");

            builder.HasKey(ck => new { ck.ReviewedInternshipId, ck.ReviewerId });

            builder.HasOne(e => e.ReviewedInternship)
                .WithMany(f => f.Reviewers)
                .HasForeignKey(e => e.ReviewedInternshipId);

            builder.HasOne(e => e.Reviewer)
                .WithMany(f => f.AssignedInternshipsToReview)
                .HasForeignKey(e => e.ReviewerId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.Feedback).HasColumnName("Feedback");
            builder.Property(e => e.StateOfTeacher).HasColumnName("State_of_teacher")
                .HasDefaultValue(InternshipState.InReviewByTeacher);
        }
    }
}
