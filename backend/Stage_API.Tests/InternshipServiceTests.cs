using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Stage_API.Business;
using Stage_API.Data;
using Stage_API.Domain;
using Stage_API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stage_API.Tests
{
    public class InternshipServiceTests
    {
        InternshipContext _context;

        InternshipService sut;

        InternshipCreationDto dto;

        Internship internship;

        User userTeacher;

        User userTeacher2;

        List<ReviewerInternships> reviewerList;

        InternshipFeedbackCoordinatorDto IFCDto;

        InternshipFeedbackTeacherDto IFTDto;

        InternshipUpdate updateDto;

        // Mock<UserManager<User>> moqUserManager;

        Company company;

        [SetUp]
        public void Setup()
        {
            _context = ContextHelper.GetDatabaseContext();

            sut = new InternshipService(_context);

            dto = new InternshipCreationDto
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
            };         

            for (int i = 0; i < 10; i++)
            {
                dto.RequiredFieldsOfStudy.Add("ReqFOS" + i);
                dto.AssignedStudents.Add("AssignedStudent" + i);
                dto.Environment.Add("Environment" + i);
                dto.Activities.Add("Activity" + i);
                dto.PeriodOfInternship.Add("Period" + i);
            }

            internship = new Internship
            {
                CreatorId = Guid.NewGuid(),
                RequiredFieldsOfStudy = dto.RequiredFieldsOfStudy,
                AssignedStudents = dto.AssignedStudents,
                Environment = dto.Environment,
                TechnicalDescription = "TechnicalDescription",
                ExtraRequirements = "ExtraRequirements",
                ResearchTheme = "ResearchTheme",
                Activities = dto.Activities,
                RequiredStudentsAmount = 2,
                AdditionalRemarks = "AdditionalRemarks",
                Id = Guid.NewGuid(),
                InternshipState = 0,
                Periods = dto.PeriodOfInternship,
                Description = "Description",
                DateOfState = DateTime.UtcNow
            };

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

            userTeacher2 = new User
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

            company = new Company
            {
                Id = internship.CreatorId
            };


            ReviewerInternships reviewerInternships = new ReviewerInternships
            {
                ReviewedInternship = internship,
                Reviewer = userTeacher
            };

            reviewerList = new List<ReviewerInternships> { reviewerInternships };

            internship.Reviewers = reviewerList;

             IFCDto = new InternshipFeedbackCoordinatorDto
            {
                Feedback = "coordinator",
                InternshipId = internship.Id,
                ReviewedState = InternshipState.ApprovedByAll
             };

            IFTDto = new InternshipFeedbackTeacherDto
            {
                Feedback = "teacher",
                InternshipId = internship.Id,
                TeacherId = userTeacher.Id
            };

            updateDto = new InternshipUpdate
            {
                ID = internship.Id,
                ResearchTheme = "ResearchTheme",
                Description = "DescriptionUpdate",
                Activities = new List<string>() { "test" },
                AdditionalRemarks = "test",
                AssignedStudents = new List<string>() { "Jan" },
                Environment = new List<string>() { "test" },
                ExtraRequirements = "test",
                PeriodOfInternship = new List<string>() { "test" },
                RequiredFieldsOfStudy = new List<string>() { "AON" },
                RequiredStudentsAmount = 1,
                TechnicalDescription = "test",
                Title = "test"
            };

            _context.Add(reviewerInternships);
             _context.Add(userTeacher);
             _context.Add(internship);
             _context.SaveChanges();

        }

        [Test]
        public async Task GetAllShould_ReturnTheCorrectValues()
        {
            var resultList = await sut.GetAll();
            // Checking if contains due to seeded internships

            Assert.True(resultList.Contains(internship));
        }


        [Test]
        public async Task InsertShould_AddNewInternshipToTheContext()
        {

            await sut.Insert(dto,  company);

            CollectionAssert.AreEqual(dto.PeriodOfInternship, (await _context.Internships.FirstOrDefaultAsync(x => x.Periods == dto.PeriodOfInternship)).Periods);
            CollectionAssert.AreEqual(dto.Environment, (await _context.Internships.FirstOrDefaultAsync(x => x.Environment == dto.Environment)).Environment);
            CollectionAssert.AreEqual(dto.AssignedStudents, (await _context.Internships.FirstOrDefaultAsync(x => x.AssignedStudents == dto.AssignedStudents)).AssignedStudents);
            CollectionAssert.AreEqual(dto.RequiredFieldsOfStudy, (await _context.Internships.FirstOrDefaultAsync(x => x.RequiredFieldsOfStudy == dto.RequiredFieldsOfStudy)).RequiredFieldsOfStudy);
            CollectionAssert.AreEqual(dto.Activities, (await _context.Internships.FirstOrDefaultAsync(x => x.Activities == dto.Activities)).Activities);
            Assert.AreEqual(dto.Description, (await _context.Internships.FirstOrDefaultAsync(x => x.Description == dto.Description)).Description);
        }

        [Test]
        public async Task GetShould_ReturnTheCorrectValue()
        {


            Assert.AreEqual(internship, await sut.Get(internship.Id));
        }


        [Test]
        public async Task DeleteShould_DropTheGivenInternship()
        {
            await sut.Delete(internship.Id);

            Assert.AreEqual(null, await _context.FindAsync<Internship>(internship.Id));
        }

        [Test]
        public async Task GetAllReviewerInternshipsShould_ReturnTheCorrectValues()
        {
            CollectionAssert.AreEqual(reviewerList, await sut.GetAllReviewerInternships(internship.Id));
        }

        [Test]
        public async Task AssignReviewerShould_AddAReviewerToInternshipAndReturnTrue()
        {
            List<Guid> guidList = new List<Guid>
            {
                userTeacher2.Id
            };

            var response = await sut.AssignReviewer(internship.Id, guidList);

            var reviewersFromInternship = _context.ReviewerInternships.Where(x => x.ReviewerId == userTeacher2.Id);

            CollectionAssert.AreEqual(internship.Reviewers, reviewersFromInternship);
            Assert.AreEqual(true, response);
        }

        [Test]
        public async Task GetAllTeachersForInternshipShould_ReturnTheCorrectValue()
        {
            List<User> userList = new List<User>
            {
                userTeacher
            };

            CollectionAssert.AreEqual(userList, await sut.GetAllTeachersForInternship(internship.Id));
        }

        [Test]
        public async Task GetAllInternshipsOfTeacherShould_ReturnTheCorrectValue()
        {
            await sut.AssignReviewer(internship.Id, new List<Guid> {userTeacher.Id});
            var reviewerInternship = _context.ReviewerInternships.Where(x => x.ReviewerId == userTeacher.Id && x.ReviewedInternshipId == internship.Id).FirstOrDefault();
            reviewerInternship.StateOfTeacher = InternshipState.InReviewByTeacher;
            _context.Update(reviewerInternship);
            await _context.SaveChangesAsync();

            var list = await sut.GetAllInternshipsOfTeacher(userTeacher.Id);
            Assert.True(list.Contains(internship));
        }

        [Test]
        public async Task FeedBackTeacherShould_AddFeedbackToTheInternshipAndReturnTrue()
        {
            var response = await sut.FeedbackTeacher(IFTDto);

            Assert.AreEqual(true, response);
        }

        [Test]
        public async Task FeedBackCoordinatorShould_AddFeedbackToTheInternshipAndReturnTrue()
        {
            var response = await sut.FeedbackCoordinator(IFCDto);

            Assert.AreEqual(true, response);
        }

        [Test]
        public void IsReviewedByAllTeachersShould_ShouldReturnTheCorrectValue()
        {
            var response = sut.IsReviewedByAllTeachers(reviewerList);

            Assert.AreEqual(true, response);
        }

        [Test]
        public async Task UpdateShould_PassOnTheNewValuesIntoTheExistingObject()
        {
            var existing = await sut.Get(updateDto.ID);
            existing.InternshipState = InternshipState.RejectedByAll;
            _context.Update(existing);
            await _context.SaveChangesAsync();

            var response = await sut.Update(updateDto, company);

            Assert.AreEqual(response.Description, "DescriptionUpdate");
        }

    }
}
