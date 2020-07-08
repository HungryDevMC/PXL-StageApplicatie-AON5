using Stage_API.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stage_API.Dto
{
    public class InternshipUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        public ICollection<ReviewerInternships> Reviewers { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> RequiredFieldsOfStudy { get; set; }

        public InternshipState InternshipState { get; set; }

        public ICollection<string> AssignedStudents { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateConfirmed { get; set; }

        public DateTime DateAssignedToReviewer { get; set; }

        public string Environment { get; set; }

        public string EnvironmentDetails { get; set; }

        public string AdditionalConditions { get; set; }

        public string ResearchTheme { get; set; }

        public string Expectations { get; set; }

        public short AllowedStudents { get; set; }

        public string AddtionalRemarks { get; set; }

        public short PeriodOfInternship { get; set; }

        public string Description { get; set; }

        public string InternshipLocation { get; set; }

        public string Feedback { get; set; }

        public static implicit operator InternshipUpdateDto(InternshipUpdate v)
        {
            throw new NotImplementedException();
        }
    }
}
