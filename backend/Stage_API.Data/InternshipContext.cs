using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stage_API.Domain;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Stage_API.Data
{
    public class InternshipContext : IdentityDbContext<User, Role, Guid>
    {
        public InternshipContext(DbContextOptions<InternshipContext> options): base(options) { }

        #region Config
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var coordinator = new User
            {
                UserName = "coordinator@pxl.be",
                Email = "coordinator@pxl.be",
                NormalizedEmail = "COORDINATOR@PXL.BE",
                NormalizedUserName = "COORDINATOR@PXL.BE",
                FirstName = "Coordinator",
                City = "",
                DateAdded = DateTime.UtcNow,
                LastName = "Achternaam",
                PhoneNumber = "",
                StreetName = "",
                HouseNumber = 0,
                ZipCode = 0,
                AssignedInternshipsToReview = null,
                IsValidated = true,
                EmailConfirmed = true
            };
            var teacher1 = new User
            {
                UserName = "jan.willekens@pxl.be",
                Email = "jan.willekens@pxl.be",
                FirstName = "Jan",
                City = "",
                DateAdded = DateTime.UtcNow,
                LastName = "Willekens",
                PhoneNumber = "",
                StreetName = "",
                HouseNumber = 0,
                ZipCode = 0,
                AssignedInternshipsToReview = null,
                NormalizedEmail = "JAN.WILLEKENS@PXL.BE",
                NormalizedUserName = "JAN.WILLEKENS@PXL.BE",
                IsValidated = true,
                EmailConfirmed = true
            };

            var teacher2 = new User
            {
                UserName = "nele.custers@pxl.be",
                Email = "nele.custers@pxl.be",
                FirstName = "Nele",
                City = "",
                DateAdded = DateTime.UtcNow,
                LastName = "Custers",
                PhoneNumber = "",
                StreetName = "",
                HouseNumber = 0,
                ZipCode = 0,
                AssignedInternshipsToReview = null,
                NormalizedEmail = "NELE.CUSTERS@PXL.BE",
                NormalizedUserName = "NELE.CUSTERS@PXL.BE",
                IsValidated = true,
                EmailConfirmed = true
            };

            var teacher3 = new User
            {
                UserName = "bram.heyns@pxl.be",
                Email = "bram.heyns@pxl.be",
                FirstName = "Bram",
                City = "",
                DateAdded = DateTime.UtcNow,
                LastName = "Heyns",
                PhoneNumber = "",
                StreetName = "",
                HouseNumber = 0,
                ZipCode = 0,
                AssignedInternshipsToReview = null,
                NormalizedEmail = "BRAM.HEYNS@PXL.BE",
                NormalizedUserName = "BRAM.HEYNS@PXL.BE",
                IsValidated = true,
                EmailConfirmed = true
            };

            var student = new User
            {
                UserName = "robin.peeters@student.pxl.be",
                Email = "robin.peeters@student.pxl.be",
                FirstName = "Robin",
                City = "",
                DateAdded = DateTime.UtcNow,
                LastName = "Peeters",
                PhoneNumber = "",
                StreetName = "",
                HouseNumber = 0,
                ZipCode = 0,
                AssignedInternshipsToReview = null,
                NormalizedEmail = "ROBIN.PEETERS@STUDENT.PXL.BE",
                NormalizedUserName = "ROBIN.PEETERS@STUDENT.PXL.BE",
                IsValidated = true,
                EmailConfirmed = true
            };

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            coordinator.PasswordHash = hasher.HashPassword(coordinator, "Abcd1234$");
            teacher1.PasswordHash = hasher.HashPassword(teacher1, "Abcd1234$");
            teacher2.PasswordHash = hasher.HashPassword(teacher2, "Abcd1234$");
            teacher3.PasswordHash = hasher.HashPassword(teacher3, "Abcd1234$");
            student.PasswordHash = hasher.HashPassword(student, "Abcd1234$");

            coordinator.Id = Guid.NewGuid();
            teacher1.Id = Guid.NewGuid();
            teacher2.Id = Guid.NewGuid();
            teacher3.Id = Guid.NewGuid();
            student.Id = Guid.NewGuid();

            builder.Entity<User>().HasData(
            coordinator, teacher1, teacher2, teacher3, student
            );

            var coordinatorRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "coordinator",
                NormalizedName = "COORDINATOR",
                ConcurrencyStamp = null
            };

            var teacherRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "teacher",
                NormalizedName = "TEACHER",
                ConcurrencyStamp = null
            };

            var studentRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "student",
                NormalizedName = "STUDENT",
                ConcurrencyStamp = null
            };

            builder.Entity<Role>().HasData(
                coordinatorRole, teacherRole, studentRole
                );

            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = coordinatorRole.Id,
                    UserId = coordinator.Id
                },
                new IdentityUserRole<Guid>
                {
                    RoleId = teacherRole.Id,
                    UserId = teacher1.Id
                },
                new IdentityUserRole<Guid>
                {
                    RoleId = teacherRole.Id,
                    UserId = teacher2.Id
                },
                new IdentityUserRole<Guid>
                {
                    RoleId = teacherRole.Id,
                    UserId = teacher3.Id
                },
                new IdentityUserRole<Guid>
                {
                    RoleId = studentRole.Id,
                    UserId = student.Id
                }
                );

            builder.Entity<Internship>().HasData(
                new Internship
                {
                    Id = Guid.NewGuid(),
                    Title = "Opstellen van een netwerk",
                    RequiredFieldsOfStudy = new Collection<string>() { "SNB" },
                    Description = "De student zal het bedrijf helpen met het configureren en het onderhouden van een netwerk.",
                    Environment = new Collection<string>() { "Systemen en netwerken" },
                    TechnicalDescription = "Geen.",
                    ExtraRequirements = "De student moet vlot Engels spreken.",
                    ResearchTheme = "Probleemoplossend denken.",
                    Activities = new Collection<string>() { "Sollicitatiegesprek", "CV" },
                    RequiredStudentsAmount = 1,
                    AdditionalRemarks = "Geen.",
                    Periods = new Collection<string>() { "Semester 1 (oktober - januari)" },
                    AssignedStudents = null,
                    CreatorId = Guid.NewGuid(),
                    Feedback = null,
                    DateCreated = DateTime.UtcNow,
                    DateOfState = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    InternshipState = InternshipState.New,
                    Reviewers = null,
                    UserCreated = Guid.NewGuid(),
                    UserUpdated = Guid.NewGuid()
                },
                new Internship
                {
                    Id = Guid.NewGuid(),
                    Title = "Software testing",
                    RequiredFieldsOfStudy = new Collection<string>() { "SWM" },
                    Description = "De studenten zullen bestaande software testen om te zien of ze voldoen aan de gevraagde criteria.",
                    Environment = new Collection<string>() { "Software testing" },
                    TechnicalDescription = "Geen.",
                    ExtraRequirements = "De student moet vlot Engels spreken.",
                    ResearchTheme = "Software testing",
                    Activities = new Collection<string>() { "CV" },
                    RequiredStudentsAmount = 2,
                    AdditionalRemarks = "Geen.",
                    Periods = new Collection<string>() { "Semester 1 (oktober - januari)", "Semester 2(februari - juni)" },
                    AssignedStudents = null,
                    CreatorId = Guid.NewGuid(),
                    Feedback = null,
                    DateCreated = DateTime.UtcNow,
                    DateOfState = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    InternshipState = InternshipState.InReviewByTeacher,
                    Reviewers = null,
                    UserCreated = Guid.NewGuid(),
                    UserUpdated = Guid.NewGuid()
                },
                new Internship
                {
                    Id = Guid.NewGuid(),
                    Title = "Web development",
                    RequiredFieldsOfStudy = new Collection<string>() { "AON" },
                    Description = "De student help met het ontwikkelen van een frontend.",
                    Environment = new Collection<string>() { "Web" },
                    TechnicalDescription = "Geen.",
                    ExtraRequirements = "De student moet vlot Engels spreken.",
                    ResearchTheme = "Development",
                    Activities = new Collection<string>() { "CV" },
                    RequiredStudentsAmount = 1,
                    AdditionalRemarks = "Geen.",
                    Periods = new Collection<string>() { "Semester 2(februari - juni)" },
                    AssignedStudents = null,
                    CreatorId = Guid.NewGuid(),
                    Feedback = null,
                    DateCreated = DateTime.UtcNow,
                    DateOfState = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    InternshipState = InternshipState.ApprovedByAll,
                    Reviewers = null,
                    UserCreated = Guid.NewGuid(),
                    UserUpdated = Guid.NewGuid()
                }
                );

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region DbSets
        public DbSet<Internship> Internships { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ReviewerInternships> ReviewerInternships { get; set; }
        #endregion
    }
}
