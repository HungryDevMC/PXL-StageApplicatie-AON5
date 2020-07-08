using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stage_API.Domain;
using System;

namespace Stage_API.Data.Configuration.InternshipConfiguration
{
    public class InternshipConfiguration : IEntityTypeConfiguration<Internship>
    {
        public void Configure(EntityTypeBuilder<Internship> builder)
        {
            builder.ToTable("Internships");

            builder.Property(e => e.RequiredFieldsOfStudy).HasColumnName("Fields_of_study")
                .HasConversion(
                c => string.Join(',', c),
                c => c.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .IsRequired();

            builder.Property(e => e.Title).HasColumnName("Title")
                .IsRequired();

            builder.Property(e => e.Description).HasColumnName("Description")
                .IsRequired();

            builder.Property(e => e.Environment).HasColumnName("Environment")
                .HasConversion(
                c => string.Join(',', c),
                c => c.Split(',', StringSplitOptions.RemoveEmptyEntries)
                )
                .IsRequired();

            builder.Property(e => e.TechnicalDescription).HasColumnName("Technical_description")
                .IsRequired();

            builder.Property(e => e.ExtraRequirements).HasColumnName("Extra_requirements")
                .HasDefaultValue("Geen");

            builder.Property(e => e.ResearchTheme).HasColumnName("Research_theme")
                .IsRequired();

            builder.Property(e => e.Activities).HasColumnName("Activites")
                .HasConversion(
                    c => string.Join(',', c),
                    c => c.Split(',', StringSplitOptions.RemoveEmptyEntries)
                );

            builder.Property(e => e.RequiredStudentsAmount).HasColumnName("Required_student_amount")
                .IsRequired();

            builder.Property(e => e.AssignedStudents).HasColumnName("Assigned_students")
                .HasConversion(
                c => string.Join(',', c),
                c => c.Split(',', StringSplitOptions.RemoveEmptyEntries)
                );

            builder.Property(e => e.AdditionalRemarks).HasColumnName("Additional_remarks")
                .HasDefaultValue("Geen");

            builder.Property(e => e.Periods).HasColumnName("Periods")
                .HasConversion(
                c => string.Join(',', c),
                c => c.Split(',', StringSplitOptions.RemoveEmptyEntries)
                ).IsRequired();

            builder.Property(e => e.InternshipState).HasColumnName("Internship_state")
                .HasDefaultValue(InternshipState.New);

            builder.Property(e => e.DateOfState).HasColumnName("Date_of_state")
                .HasDefaultValue(DateTime.UtcNow)
                .HasColumnType("datetime2");

            builder.Property(e => e.Feedback).HasColumnName("Feedback");



        }
    }
}
