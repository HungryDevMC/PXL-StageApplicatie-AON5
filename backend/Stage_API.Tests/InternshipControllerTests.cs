using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Stage_API.Business.Abstractions;
using Stage_API.Controllers;
using Stage_API.Data;
using Stage_API.Domain;
using Stage_API.Dto;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stage_API.Tests
{
    public class InternshipControllerTests
    {

        Mock<IInternshipService> moqInternshipService;
        Mock<IUserService> moqUserService;
        InternshipController sut;
        InternshipAssign assignDto;
        Mock<UserManager<User>> moqUserManager;
        InternshipContext _context;
        User userTeacher;
        Internship internship;
        List<ReviewerInternships> reviewerList;
        Role roleTeacher;

        [SetUp]
        public void Setup()
        {
            _context = ContextHelper.GetDatabaseContext();

            moqInternshipService = new Mock<IInternshipService>();
            moqUserService = new Mock<IUserService>();
            
            userTeacher = new User
            {
                Id = Guid.NewGuid(),
                City = "City",
                Email = "John.doe@pxl.be",
                FirstName = "John",
                HouseNumber = 18,
                LastName = "Doe",
                PasswordHash = Guid.NewGuid().ToString(),
                PhoneNumber = "+3259874896",
                ZipCode = 7890,
                UserName = "John.doe@pxl.be",
                StreetName = "Easy Street"
            };

            internship = new Internship
            {

                RequiredFieldsOfStudy = new List<string>(),
                AssignedStudents = new List<string>(),
                Environment = new List<string>(),
                TechnicalDescription = "TechnicalDescription",
                ExtraRequirements = "ExtraRequirements",
                ResearchTheme = "ResearchTheme",
                Activities = new List<string>(),
                RequiredStudentsAmount = 2,
                AdditionalRemarks = "AdditionalRemarks",
                Id = Guid.NewGuid(),
                InternshipState = 0,
                Periods = new List<string>(),
                Description = "Description",
                DateOfState = DateTime.UtcNow,
            };

            roleTeacher = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Teacher",
                NormalizedName = "TEACHER"
            };

            ReviewerInternships reviewerInternships = new ReviewerInternships
            {
                ReviewedInternship = internship,
                Reviewer = userTeacher
            };

            reviewerList = new List<ReviewerInternships> { reviewerInternships };

            internship.Reviewers = reviewerList;

            var store = new Mock<IUserStore<User>>();
            

            moqUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            moqUserManager.Object.UserValidators.Add(new UserValidator<User>());
            moqUserManager.Object.PasswordValidators.Add(new PasswordValidator<User>());



            sut = new InternshipController(moqInternshipService.Object, moqUserService.Object,moqUserManager.Object);

            assignDto = new InternshipAssign
            {
                InternshipID = Guid.NewGuid(),
                Teachers = new List<Guid> { Guid.NewGuid() }
            };

            _context.Add(userTeacher);
            _context.Roles.Add(roleTeacher);
            _context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                RoleId = roleTeacher.Id,
                UserId = userTeacher.Id
            });

            _context.SaveChanges();
        }
          
        [Test]
        public async Task UpdateInternshipShould_ReturnOkObjectResult()
        {
            moqUserManager.Setup(_ => _.GetUserAsync(It.IsAny<ClaimsPrincipal>())).
               ReturnsAsync(new User());

            moqUserService.Setup(_ => _.GetCompany(It.IsAny<User>()))
                .ReturnsAsync(new Company());

            moqInternshipService.Setup(_ => _.Update(It.IsAny<InternshipUpdate>(), It.IsAny<Company>()))
                .ReturnsAsync(new Internship());

            InternshipUpdate model = new InternshipUpdate
            {
                Activities = new List<string>() { "abc" },
                AdditionalRemarks = "abc",
                AssignedStudents = new List<string>() { "abc" },
                Description = "abc",
                Environment = new List<string>() { "abc" },
                ExtraRequirements = "abc",
                PeriodOfInternship = new List<string>() { "abc" },
                RequiredFieldsOfStudy = new List<string>() { "abc" },
                RequiredStudentsAmount = 1,
                ResearchTheme = "abc",
                TechnicalDescription = "abc",
                Title = "abc",
                ID = Guid.NewGuid()
            };

            var result = await sut.UpdateInternship(model);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task RemoveInternshipShould_ReturnTask()
        {
            moqInternshipService.Setup(_ => _.Get(It.IsAny<Guid>()))
                .ReturnsAsync(new Internship());

            moqInternshipService.Setup(_ => _.Delete(It.IsAny<Guid>()));

            var result = await sut.RemoveInternship(Guid.NewGuid());

            Assert.That(result, Is.TypeOf<OkResult>());

        }

        [Test]
        public async Task PostShould_InsertANewInternship()
        {
            moqInternshipService.Setup(_ => _.Insert(It.IsAny<InternshipCreationDto>(),It.IsAny<Company>()))
            .ReturnsAsync(new Internship());

            moqUserManager.Setup(_ => _.GetUserAsync(It.IsAny<ClaimsPrincipal>())).
                ReturnsAsync(new User());

            moqUserService.Setup(_ => _.GetCompany(It.IsAny<User>())).
                ReturnsAsync(new Company());

            var result = await sut.Create(new InternshipCreationDto
            {
                RequiredFieldsOfStudy = new List<string>(),
                AssignedStudents = new List<string>(),
                Environment = new List<string>(),
                TechnicalDescription = "TechnicalDescription",
                ExtraRequirements = "ExtraRequirements",
                ResearchTheme = "ResearchTheme",
                Activities = new List<string>(),
                RequiredStudentsAmount = 2,
                AdditionalRemarks = "AdditionalRemarks",
                PeriodOfInternship = new List<string>(),
                Description = "Description"
            });

            Assert.That(result, Is.TypeOf<ActionResult<Internship>>());
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAllShould_ReturnCollectionOfInternships()
        {
            moqInternshipService.Setup(_ => _.GetAllInternshipsOfTeacher(It.IsAny<Guid>()))
          .ReturnsAsync(new List<Internship>());

            moqUserManager.Setup(_ => _.GetUserAsync(It.IsAny<ClaimsPrincipal>())).
                ReturnsAsync(userTeacher);

            moqUserService.Setup(_ => _.GetRoleName(It.IsAny<Guid>()))
                 .ReturnsAsync(roleTeacher.Name.ToLower());

            moqInternshipService.Setup(_ => _.GetAll())
                .ReturnsAsync(new List<Internship>());

            var result = await sut.GetAll();

            Assert.That(result, Is.TypeOf<ActionResult<ICollection<Internship>>>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetInternshipsOfTeacherShould_ReturnCollectionOfInternships()
        {
            moqInternshipService.Setup(_ => _.GetAllInternshipsOfTeacher(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Internship>());

            moqUserManager.Setup(_ => _.GetUserAsync(It.IsAny<ClaimsPrincipal>())).
                ReturnsAsync(userTeacher);

            moqUserService.Setup(_ => _.GetRoleName(It.IsAny<Guid>()))
                 .ReturnsAsync(roleTeacher.Name.ToLower());

            var result = await sut.GetInternshipsOfTeacher(userTeacher.Id);

            Assert.That(result, Is.TypeOf<ActionResult<ICollection<Internship>>>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetTeachersShould_ReturnACollectionOfUserReadDtos()
        {
            moqUserService.Setup(_ => _.GetUsersByRole("TEACHER"))
                .ReturnsAsync(new List<UserReadDto>());

            var result = await sut.GetTeachers();

            Assert.That(result, Is.TypeOf<ActionResult<ICollection<UserReadDto>>>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task AssignTeachersShould_AssignANewTeacherAndReturnTrue()
        {
            moqInternshipService.Setup(_ => _.AssignReviewer(It.IsAny<Guid>(), It.IsAny<ICollection<Guid>>()))
           .ReturnsAsync(true);

            var result = await sut.AssignTeachers(new InternshipAssign
            {
                InternshipID = Guid.NewGuid(),
                Teachers = new List<Guid> { Guid.NewGuid() }
            });

            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public async Task FeedbackTeacherShould_AddFeedbackTeacherAndReturnTrue()
        {
            moqInternshipService.Setup(_ => _.FeedbackTeacher(It.IsAny<InternshipFeedbackTeacherDto>()))
                .ReturnsAsync(new Boolean());
            var result = await sut.FeedbackTeacher(new InternshipFeedbackTeacherDto
            {
                Feedback = "teacher",
                InternshipId = Guid.NewGuid(),
                ReviewedState = 0,
                TeacherId = Guid.NewGuid()
            });

            Assert.That(result, Is.TypeOf<ActionResult<bool>>());
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task FeedbackCoordinatorShould_AddFeedbackCoordinatorAndReturnTrue()
        {
            moqInternshipService.Setup(_ => _.FeedbackCoordinator(It.IsAny<InternshipFeedbackCoordinatorDto>()))
                .ReturnsAsync(new Boolean());

            var result = await sut.FeedbackCoordinator(new InternshipFeedbackCoordinatorDto
            {
                Feedback = "coordinator",
                InternshipId = Guid.NewGuid(),
            });

            Assert.That(result, Is.TypeOf<ActionResult<bool>>());
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

    }
}
