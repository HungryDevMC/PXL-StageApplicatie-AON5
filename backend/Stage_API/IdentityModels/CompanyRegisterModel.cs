using System.ComponentModel.DataAnnotations;

namespace Stage_API.IdentityModels
{
    public class CompanyRegisterModel
    {
        public string CompanyName { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public int EmployeeCount { get; set; }

        public int ITEmployeeCount { get; set; }

        public int SupportingITEmployees { get; set; }

        public float Lat1 { get; set; }

        public float Lng1 { get; set; }

        public float Lat2 { get; set; }

        public float Lng2 { get; set; }

        public string Contact_Title { get; set; }

        public string Contact_Name { get; set; }

        public string Contact_Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Contact_Email { get; set; }

        public string Contact_Number { get; set; }

        public string Company_Title { get; set; }

        public string Company_Name { get; set; }

        public string Company_Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Company_Email { get; set; }

        public string Company_Number { get; set; }
    }
}
