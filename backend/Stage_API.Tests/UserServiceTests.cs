using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class UserServiceTests
    {
        User userTeacher;
        User userCompany;
        User userStudent;

        Company company;
        CompanyUpdate companyUpdate;

        Role roleCompany;
        Role roleStudent;
        Role roleTeacher;

        UserReadDto dtoTeacher;
        UserReadDto dtoCompany;
        UserReadDto dtoStudent;

        ChangePasswordDto pwDto;
        UserValidateDto userValidateDto;

        UserUpdate updateUser;

        InternshipContext _context;

        UserService sut;

        Internship internship;

        [SetUp]
        public void Setup()
        {
            _context = ContextHelper.GetDatabaseContext();
            
            sut = new UserService(_context);  

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
                StreetName = "Easy Street",
                IsValidated = false
            };

            company = new Company
            {
                CompanyTitle = "Test inc."
            };

            dtoTeacher = new UserReadDto
            {
                FirstName = userTeacher.FirstName,
                LastName = userTeacher.LastName,
                Id = userTeacher.Id
            };

            userCompany = new User
            {
                Id = Guid.NewGuid(),
                City = "City",
                Email = "Johnnie.doe@company.com",
                FirstName = "Johnnie",
                HouseNumber = 19,
                LastName = "Doe",
                PasswordHash = Guid.NewGuid().ToString(),
                PhoneNumber = "+3259874896",
                ZipCode = 7890,
                UserName = "Johnnie.doe@company.com",
                StreetName = "Easy Street",
                IsValidated = false
            };

            dtoCompany = new UserReadDto
            {
                FirstName = userCompany.FirstName,
                LastName = userCompany.LastName,
                Id = userCompany.Id
            };

            userStudent = new User
            {
                Id = Guid.NewGuid(),
                City = "City",
                Email = "Henk.doe@student.pxl.be",
                FirstName = "Henk",
                HouseNumber = 17,
                LastName = "Doe",
                PasswordHash = Guid.NewGuid().ToString(),
                PhoneNumber = "+3259874896",
                ZipCode = 7890,
                UserName = "Henk.doe@pxl.be",
                StreetName = "Easy Street",
                IsValidated = false
            };

            dtoStudent = new UserReadDto
            {
                FirstName = userStudent.FirstName,
                LastName = userStudent.LastName,
                Id = userStudent.Id
            };

            roleCompany = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Company",
                NormalizedName = "COMPANY"
            };

            roleStudent = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Student",
                NormalizedName = "STUDENT"
            };

            roleTeacher = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Teacher",
                NormalizedName = "TEACHER"
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
                Periods = new List<string>(),
                Description = "Description",
                DateCreated = DateTime.UtcNow,
                DateOfState = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                InternshipState = 0,
                Reviewers = new List<ReviewerInternships>{
                    new ReviewerInternships
                    {
                        ReviewedInternship = internship,
                        Reviewer = userTeacher
                    }
                }
            };

            updateUser = new UserUpdate
            {
                FieldOfStudy = "UpdatedFOS"
            };

            userValidateDto = new UserValidateDto
            {
                Id = userCompany.Id,
                Validated = true
            };

            pwDto = new ChangePasswordDto
            {
                OldPassword = userStudent.PasswordHash,
                NewPassword = "String*-123"
            };

            companyUpdate = new CompanyUpdate
            {
                CompanyName = "Updated"
            };

            _context.Add(company);

            _context.Add(userCompany);
            _context.Roles.Add(roleCompany);
            _context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>
            {
                RoleId = roleCompany.Id,
                UserId = userCompany.Id
            });
            _context.Add(userTeacher);
            _context.Roles.Add(roleTeacher);
            _context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>
            {
                RoleId = roleTeacher.Id,
                UserId = userTeacher.Id
            });
            _context.Add(userStudent);
            _context.Roles.Add(roleStudent);
            _context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>
            {
                RoleId = roleStudent.Id,
                UserId = userStudent.Id
            });
            _context.Add(internship);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllCompaniesShould_ReturnTheCorrectValues()
        {
            var companies = (await sut.GetUsersByRole("COMPANY")).ToList();

            var userCompList = new List<UserReadDto> { };
            userCompList.Add(dtoCompany);

            Assert.AreEqual(companies.First().Id, userCompList.First().Id);
        }

        [Test]
        public async Task GetRoleNameShould_ReturnTheCorrectValue()
        {
            var roleName = await sut.GetRoleName(userTeacher.Id);
            
            Assert.AreEqual(roleName, roleTeacher.Name.ToUpper());
        }

        [Test]
        public async Task AssignCompany_ShouldAddACompanyToUser()
        {
            Guid randomGuid = Guid.NewGuid();
            await sut.AssignCompany(userCompany, randomGuid);

            Assert.AreEqual(randomGuid, userCompany.CompanyId);
        }

        [Test]
        public async Task CreateCompanyShould_AddACompanyToTheContext()
        {
            var newComp = company;
            Guid oldGuid = company.Id;
            newComp.Id = Guid.NewGuid();
            await sut.CreateCompany(newComp);

            newComp.Id = oldGuid;
            Assert.AreEqual(newComp, company);
        }

        [Test]
        public async Task GetCompanyShould_ReturnTheCorrectValues()
        {
            await sut.AssignCompany(userCompany, Guid.NewGuid());

            var response = await sut.GetCompany(userCompany);
        }

        [Test]
        public async Task ValidateUserShould_UpdateUsers()
        {
            await sut.ValidateUser(userValidateDto);

            Assert.AreEqual(userValidateDto.Validated, userCompany.IsValidated);
        }

        [Test]
        public async Task GetUnvalidatedShould_ReturnTheCorrectValues()
        {
            List<UserReadDto> userReadDtos = new List<UserReadDto>
            {
                dtoCompany,
                dtoStudent,
                dtoTeacher
            };

            var result = await sut.GetUnvalidated();

            CollectionAssert.AreEquivalent(result, userReadDtos);
        }

        [Test]
        public async Task UpdateUserShould_PassOnTheValuesToTheObjectInTheDatabase()
        {
            await sut.UpdateUser(userStudent, updateUser);

            Assert.AreEqual(userStudent.FieldOfStudy, "UpdatedFOS");
        }

        [Test]
        public async Task UpdateCompanyShould_PassOnTheValuesToTheObjectInTheDatabase()
        {
            await sut.UpdateCompany(company, companyUpdate);

            Assert.AreEqual(company.Name, "Updated");
        }
    }
}
