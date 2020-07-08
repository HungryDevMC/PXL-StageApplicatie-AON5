using StageAPI.Extensions;
using System.ComponentModel.DataAnnotations;

namespace StageAPI.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [Password(ErrorMessage = "PasswordInvalid")]
        public string Password { get; set; }
    }
}
