using Stage_API.Domain;
using Stage_API.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stage_API.Business.Abstractions
{
    public interface IInternshipService
    {
        Task<Internship> Get(Guid entityId);

        Task<ICollection<Internship>> GetAll();

        Task<Internship> Insert(InternshipCreationDto entity, Company user);

        Task<Internship> Update(InternshipUpdate entity, Company user);

        Task Delete(Guid entityId);

        Task Save();

        Task<bool> AssignReviewer(Guid internshipId, ICollection<Guid> teacherIds);

        Task<ICollection<User>> GetAllTeachersForInternship(Guid internshipId);
        Task<ICollection<Internship>> GetAllInternshipsOfTeacher(Guid teacherId);
        Task<ICollection<Internship>> GetAllInternshipsOfCompany(Guid companyId);
        Task<ICollection<Internship>> GetAllInternshipsForStudent();
        Task<bool> FeedbackTeacher(InternshipFeedbackTeacherDto model);
        Task<bool> FeedbackCoordinator(InternshipFeedbackCoordinatorDto model);
    }
}

