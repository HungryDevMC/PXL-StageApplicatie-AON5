using Stage_API.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Stage_API.Dto
{
    public class InternshipFeedbackCoordinatorDto
    {
        [Required]
        public Guid InternshipId { get; set; }

        [Required]
        public string Feedback { get; set; }

        [Required]
        public InternshipState ReviewedState { get; set; }

    }
}
