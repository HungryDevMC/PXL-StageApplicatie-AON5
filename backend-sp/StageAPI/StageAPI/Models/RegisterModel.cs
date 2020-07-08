using StageAPI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StageAPI.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string Email { get; set; }

        [Required]
        [Password(ErrorMessage = "PasswordInvalid")]
        public string Password { get; set; }

        [Required]
        [Password(ErrorMessage = "PasswordInvalid")]
        public string PasswordConfirm { get; set; }
    }
}
