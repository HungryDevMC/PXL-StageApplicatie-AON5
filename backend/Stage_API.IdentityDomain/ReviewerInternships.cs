using System;

namespace Stage_API.Domain
{
    public class ReviewerInternships
    {
        public User Reviewer { get; set; }
        public Guid ReviewerId { get; set; }

        public Internship ReviewedInternship { get; set; }
        public Guid ReviewedInternshipId { get; set; }

        public InternshipState StateOfTeacher { get; set; }

        public string Feedback { get; set; }

    }
}
