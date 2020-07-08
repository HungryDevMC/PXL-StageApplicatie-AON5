using System.ComponentModel.DataAnnotations;

namespace Stage_API.Dto
{
    public class UserUpdate
    {
        [Phone]
        public string PhoneNumber { get; set; }

        public short HouseNumber { get; set; }

        public string City { get; set; }

        public short ZipCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Bus { get; set; }

        public string FieldOfStudy { get; set; }
    }
}
