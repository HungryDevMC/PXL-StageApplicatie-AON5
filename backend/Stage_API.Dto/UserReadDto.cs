using System;

namespace Stage_API.Dto
{
    public class UserReadDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid Id { get; set; }

        public override bool Equals(object other)
        {
            var obj = (UserReadDto)other;

            return obj.FirstName == FirstName && obj.LastName == LastName && obj.Id == Id;
        }
    }
}
