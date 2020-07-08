using System;
using System.Collections.Generic;

namespace Stage_API.Dto
{
    public class InternshipAssign
    {
        public Guid InternshipID { get; set; }
        public ICollection<Guid> Teachers { get; set; }
    }
}
