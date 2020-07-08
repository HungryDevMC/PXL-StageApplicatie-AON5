using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stage_API.Business.Abstractions;
using Stage_API.Data;
using Stage_API.Domain;
using Stage_API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stage_API.Business
{
    public class UserService : IUserService
    {
        private readonly InternshipContext _context;
        private readonly PasswordHasher<User> _hasher;

        public UserService(InternshipContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }
        public UserReadDto Get(Guid EntityId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<UserReadDto>> GetUsersByRole(string role)
        {
            var companyRoleId = (await _context.Roles.FirstOrDefaultAsync(x => x.NormalizedName == role.ToUpper())).Id;
            var UserIdsFromCompanies = await _context.UserRoles.Where(x => x.RoleId == companyRoleId).Select(x => x.UserId).ToListAsync();
            var DtoList = new List<UserReadDto>();
            foreach (var id in UserIdsFromCompanies)
            {
                var dto = new UserReadDto
                {
                    Id = id,
                    FirstName = (await _context.FindAsync<User>(id)).FirstName,
                    LastName = (await _context.FindAsync<User>(id)).LastName
                };

                DtoList.Add(dto);
            }
            return DtoList;
        }

        public async Task ValidateUser(UserValidateDto model)
        {
            var user = (await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id));
            user.IsValidated = model.Validated;
            _context.Update(user);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetRoleName(Guid userId)
        {
            var roleId = (await _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId)).RoleId;
            return (await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId)).NormalizedName;
        }

        public async Task<ICollection<UserReadDto>> GetUnvalidated()
        {
            var unvalidatedAccounts = (await _context.Users.Where(u => !u.IsValidated).ToListAsync());
            var unvalidatedDtos = new List<UserReadDto>();
            foreach (User user in unvalidatedAccounts)
            {
                UserReadDto userReadDto = new UserReadDto()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                unvalidatedDtos.Add(userReadDto);
            }
            return unvalidatedDtos;
        }

        public async Task AssignCompany(User user, Guid id)
        {
            user.CompanyId = id;

            _context.Update(user);
            await Save();
        }

        public async Task CreateCompany(Company company)
        {
            await _context.Companies.AddAsync(company);
            await Save();
        }

        public async Task<Company> GetCompany(User user)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == user.CompanyId);
        }

        public async Task UpdateCompany(Company company, CompanyUpdate model)
        {
            company.Name = model.CompanyName;
            company.EmployeeCount = model.EmployeeCount;
            company.ITEmployeeCount = model.ITEmployeeCount;
            company.SupportingITEmployees = model.SupportingITEmployees;
            company.Latitude1 = model.Lat1;
            company.Longitude1 = model.Lng1;
            company.Latitude2 = model.Lat2;
            company.Longitude2 = model.Lat2;
            company.ContactTitle = model.Contact_Title;
            company.CompanyTitle = model.Company_Title;

            _context.Update(company);
            await Save();
        }

        public async Task UpdateUser(User user, UserUpdate model)
        {
            user.PhoneNumber = model.PhoneNumber;
            user.HouseNumber = model.HouseNumber;
            user.City = model.City;
            user.ZipCode = model.ZipCode;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Bus = model.Bus;
            user.FieldOfStudy = model.FieldOfStudy;

            _context.Update(user);
            await Save();
        }
    }
}
