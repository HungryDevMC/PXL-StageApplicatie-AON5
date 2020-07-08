using Stage_API.Domain;
using Stage_API.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stage_API.Business.Abstractions
{
    public interface IUserService
    {

        UserReadDto Get(Guid EntityId);

        Task Save();

        Task<ICollection<UserReadDto>> GetUsersByRole(string role);
        Task<string> GetRoleName(Guid userId);

        Task ValidateUser(UserValidateDto model);
        Task<ICollection<UserReadDto>> GetUnvalidated();
		
        Task CreateCompany(Company company);

        Task AssignCompany(User user, Guid id);

        Task<Company> GetCompany(User user);

        Task UpdateCompany(Company company, CompanyUpdate model);
        Task UpdateUser(User user, UserUpdate model);
    }
}
