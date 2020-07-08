using Stage_API.Domain;
using System.Collections.Generic;

namespace StageAPI.Domain
{
    public class FieldOfStudy : Entity
    {
        public string Name { get; set; }

        public ICollection<InternshipFieldOfStudy> Internships { get; set; }
    }
}