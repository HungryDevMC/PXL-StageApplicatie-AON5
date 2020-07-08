using System;
using System.ComponentModel.DataAnnotations;

namespace Stage_API.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
