using System;
using System.ComponentModel.DataAnnotations;

namespace Stage_API.Dto
{
    public class InternshipFeedbackTeacherDto : InternshipFeedbackCoordinatorDto
    {
        [Required]
        public Guid TeacherId { get; set; }

    }
}
