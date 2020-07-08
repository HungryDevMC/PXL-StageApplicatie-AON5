using StageAPI.Extensions;
using System.ComponentModel.DataAnnotations;

namespace StageAPI.Models
{
    public class CreateProposalModel
    {
        [Required]
        public string[] Study { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string[] Environment { get;  set; }

        [Required]
        public string TechnicalDescription { get; set; }

        [Required]
        public string ExtraRequirements { get; set; }

        [Required]
        public string Theme { get; set; }

        [Required]
        public string[] Activities { get; set; }

        [Required]
        public int AmountOfStudents { get; set; }

        [Required]
        public string[] Names { get; set; }

        [Required]
        public string Remarks { get; set; }

        [Required]
        public string[] Period { get; set; }
    }
}
