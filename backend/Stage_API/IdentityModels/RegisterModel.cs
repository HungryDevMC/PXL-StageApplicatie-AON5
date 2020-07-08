using System.ComponentModel.DataAnnotations;

namespace Stage_API.IdentityModels
{
    public class RegisterModel
    {
        [Phone]
        public string PhoneNumber { get; set; }

        public short HouseNumber { get; set; }

        public string City { get; set; }

        public string StreetName { get; set; }

        public short ZipCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Bus { get; set; }

        public string FieldOfStudy { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
