using System;

namespace Stage_API.Domain
{
    public class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime DateCreated { get; set; }

        public Guid UserCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public Guid UserUpdated { get; set; }
    }
}
