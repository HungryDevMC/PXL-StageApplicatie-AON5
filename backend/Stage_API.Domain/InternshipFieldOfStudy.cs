using StageAPI.Domain;
using System;

namespace Stage_API.Domain
{
    public class InternshipFieldOfStudy
    {
        public Guid InternshipId { get; set; }
        public Internship Internship { get; set; }
        public Guid FieldOfStudyId { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
    }
}
