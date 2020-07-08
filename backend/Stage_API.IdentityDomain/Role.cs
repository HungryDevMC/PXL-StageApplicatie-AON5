using Microsoft.AspNetCore.Identity;
using System;

namespace Stage_API.Domain
{
    public class Role : IdentityRole<Guid>
    {
        public const string Teacher = "teacher";
        public const string Coordinator = "coordinator";
        public const string Student = "student";
        public const string Company = "company";
    }
}
