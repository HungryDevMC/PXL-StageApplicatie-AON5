using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Stage_API.Domain
{
    public class User : IdentityUser<Guid>
    {
        public bool Activated { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public short HouseNumber { get; set; }

        public string City { get; set; } // google table voor cities.

        public string Bus { get; set; }

        public string FieldOfStudy { get; set; }

        public string StreetName { get; set; }

        public short ZipCode { get; set; } // idem zo

        public DateTime DateAdded { get; set; }

        public ICollection<ReviewerInternships> AssignedInternshipsToReview { get; set; }

        public bool IsValidated { get; set; }

        public Guid CompanyId { get; set; }
    }
}
