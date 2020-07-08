using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stage_API.Dto
{
    public class InternshipUpdate
    {
        [Required]
        public Guid ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public ICollection<string> RequiredFieldsOfStudy { get; set; }

        [Required]
        public ICollection<string> AssignedStudents { get; set; }

        [Required]
        public ICollection<string> Environment { get; set; }

        [Required]
        public string TechnicalDescription { get; set; }

        [Required]
        public string ExtraRequirements { get; set; }

        [Required]
        public string ResearchTheme { get; set; }

        [Required]
        public ICollection<string> Activities { get; set; }

        [Required]
        public byte RequiredStudentsAmount { get; set; }

        [Required]
        public string AdditionalRemarks { get; set; }

        [Required]
        public ICollection<string> PeriodOfInternship { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
