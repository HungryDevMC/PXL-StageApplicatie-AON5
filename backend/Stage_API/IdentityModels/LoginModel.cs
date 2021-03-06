﻿using System.ComponentModel.DataAnnotations;

namespace Stage_API.IdentityModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; } 
    }
}
