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
    public class InternshipService : IInternshipService
    {
        private readonly InternshipContext _context;

        public InternshipService(InternshipContext context)
        {
            _context = context;
        }

        public async Task Delete(Guid entityId)
        {
            var res = await _context.Internships.SingleOrDefaultAsync(x => x.Id == entityId);
            _context.Remove(res);
            await Save();
        }

        public async Task<Internship> Get(Guid entityId)
        {
            var internship = await _context.FindAsync<Internship>(entityId);
            internship.Reviewers = await GetAllReviewerInternships(internship.Id);
            return internship;
        }

        public async Task<ICollection<Internship>> GetAll()
        {
            return await _context.Internships.ToListAsync();
        }

        public async Task<Internship> Insert(InternshipCreationDto entity, Company user)
        {
            var newInternship = new Internship
            {
                CreatorId = user.Id,
                Title = entity.Title,
                RequiredFieldsOfStudy = entity.RequiredFieldsOfStudy,
                Description = entity.Description,
                Environment = entity.Environment,
                TechnicalDescription = entity.TechnicalDescription,
                ExtraRequirements = entity.ExtraRequirements,
                ResearchTheme = entity.ResearchTheme,
                Activities = entity.Activities,
                RequiredStudentsAmount = entity.RequiredStudentsAmount,
                AssignedStudents = entity.AssignedStudents,
                AdditionalRemarks = entity.AdditionalRemarks,
                Periods = entity.PeriodOfInternship,
                InternshipState = InternshipState.New,
                DateOfState = DateTime.UtcNow,
                Reviewers = new List<ReviewerInternships>()
            };

            await _context.AddAsync(newInternship);
            await Save();

            return newInternship;
        }

        public async Task<Internship> Update(InternshipUpdate entity, Company user)
        {
            var existing = await Get(entity.ID);
            if (existing == null || existing?.InternshipState != InternshipState.RejectedByAll || existing.CreatorId != user.Id)
            {
                return null;
            }

            if(existing.Title == entity.Title && existing.RequiredFieldsOfStudy.SequenceEqual(entity.RequiredFieldsOfStudy) && existing.Description == entity.Description && existing.Environment.SequenceEqual(entity.Environment) && existing.TechnicalDescription == entity.TechnicalDescription && existing.ExtraRequirements == entity.ExtraRequirements
                && existing.ResearchTheme == entity.ResearchTheme && existing.RequiredStudentsAmount == entity.RequiredStudentsAmount && existing.AdditionalRemarks == entity.AdditionalRemarks && existing.Periods.SequenceEqual(entity.PeriodOfInternship) && existing.AssignedStudents.SequenceEqual(entity.AssignedStudents) && existing.Activities.SequenceEqual(entity.Activities))
            {
                return null;
            }

            existing.Title = entity.Title;
            existing.RequiredFieldsOfStudy = entity.RequiredFieldsOfStudy;
            existing.Description = entity.Description;
            existing.Environment = entity.Environment;
            existing.TechnicalDescription = entity.TechnicalDescription;
            existing.ExtraRequirements = entity.ExtraRequirements;
            existing.ResearchTheme = entity.ResearchTheme;
            existing.Activities = entity.Activities;
            existing.RequiredStudentsAmount = entity.RequiredStudentsAmount;
            existing.AssignedStudents = entity.AssignedStudents;
            existing.AdditionalRemarks = entity.AdditionalRemarks;
            existing.Periods = entity.PeriodOfInternship;

            existing.InternshipState = InternshipState.InReviewByTeacher;
            existing.DateOfState = DateTime.UtcNow;

            foreach (var reviewer in existing.Reviewers)
            {
                reviewer.Feedback = null;
                reviewer.StateOfTeacher = InternshipState.InReviewByTeacher;
            }

            _context.Update(existing);
            await Save();

            return existing;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<ReviewerInternships>> GetAllReviewerInternships(Guid internshipId)
        {
            return await _context.ReviewerInternships.Where(i => i.ReviewedInternshipId == internshipId).ToListAsync();
        }

        public async Task<bool> AssignReviewer(Guid internshipId, ICollection<Guid> teacherIds)
        {
            var internship = await Get(internshipId);

            if (internship == null || teacherIds.Count == 0)
            {
                return false;
            }

            foreach (var reviewerInternship in internship.Reviewers)
            {
                _context.Remove(reviewerInternship);
            }

            await Save();

            internship.Reviewers = new List<ReviewerInternships>();
            foreach (var teacher in teacherIds)
            {
                var reviewerInternship = new ReviewerInternships { ReviewedInternshipId = internshipId, ReviewerId = teacher };
                internship.Reviewers.Add(reviewerInternship);
            }

            internship.InternshipState = InternshipState.InReviewByTeacher;
            internship.DateOfState = DateTime.UtcNow;
            _context.Update(internship);
            await Save();
            return true;

        }

        public async Task<ICollection<User>> GetAllTeachersForInternship(Guid internshipId)
        {
            var reviewerInternships = _context.ReviewerInternships.Where(x => x.ReviewedInternshipId == internshipId);
            var teachersOfInternship = await reviewerInternships.Select(x => x.Reviewer).ToListAsync();
            return teachersOfInternship;
        }

        public async Task<ICollection<Internship>> GetAllInternshipsOfTeacher(Guid teacherId)
        {
            return await _context.ReviewerInternships.Where(t => t.ReviewerId == teacherId && t.StateOfTeacher == InternshipState.InReviewByTeacher).Select(i => i.ReviewedInternship).ToListAsync();
        }

        public async Task<ICollection<Internship>> GetAllInternshipsOfCompany(Guid companyId)
        {
            return await _context.Internships.Where(x => x.CreatorId == companyId).ToListAsync();
        }

        public async Task<ICollection<Internship>> GetAllInternshipsForStudent()
        {
            return await _context.Internships.Where(x => x.InternshipState == InternshipState.ApprovedByAll).ToListAsync();
        }

        public bool IsReviewedByAllTeachers(ICollection<ReviewerInternships> reviewerInternships)
        {
            return reviewerInternships.All(r => r.StateOfTeacher != InternshipState.InReviewByTeacher);
        }

        public async Task<bool> FeedbackTeacher(InternshipFeedbackTeacherDto model)
        {
            var internship = await Get(model.InternshipId);

            var reviewerInternship = internship.Reviewers.FirstOrDefault(x => x.ReviewerId == model.TeacherId);

            internship.Reviewers.Remove(reviewerInternship);
            reviewerInternship.Feedback = model.Feedback;
            reviewerInternship.StateOfTeacher = model.ReviewedState;

            internship.Reviewers.Add(reviewerInternship);

            if(IsReviewedByAllTeachers(internship.Reviewers))
            {
                internship.InternshipState = InternshipState.InReviewByCoordinator;
                internship.DateOfState = DateTime.UtcNow;
            }

            _context.Update(internship);
            await Save();
            return true;
        }

        public async Task<bool> FeedbackCoordinator(InternshipFeedbackCoordinatorDto model)
        {
            var internship = await Get(model.InternshipId);

            if(model.ReviewedState != InternshipState.ApprovedByAll && model.ReviewedState != InternshipState.RejectedByAll)
            {
                return false;
            }

            internship.InternshipState = model.ReviewedState;
            internship.Feedback = model.Feedback;
            _context.Update(internship);
            await Save();
            return true;
        }

    }
}
