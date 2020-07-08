using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StageAPI.Data
{
    public class Account
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountRole Role { get; set; }
    }
}
