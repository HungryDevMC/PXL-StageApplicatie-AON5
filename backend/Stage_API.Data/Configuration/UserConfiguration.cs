using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stage_API.Domain;
using System;

namespace Stage_API.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(e => e.AssignedInternshipsToReview)
                .WithOne(f => f.Reviewer).HasForeignKey(e => e.ReviewerId);

            builder.Property(e => e.IsValidated)
                .HasDefaultValue(false);

            builder.Property(e => e.CompanyId)
                .HasDefaultValue(Guid.Empty);
        }
    }
}
