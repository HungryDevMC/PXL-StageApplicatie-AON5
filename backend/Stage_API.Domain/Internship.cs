using Stage_API.Domain;
using Stage_API.IdentityDomain;
using System;
using System.Collections.Generic;

namespace StageAPI.Domain
{
    public class Internship : Entity
    {
        public ICollection<InternshipFieldOfStudy> RequiredFieldsOfStudy { get; set; }

        public InternshipState InternshipState { get; set; }

        public ICollection<User> AssignedStudents { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateConfirmed { get; set; }

        public DateTime DateAssignedToReviewer { get; set; }

        public bool IsEditable { get; set; }

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

    }
}