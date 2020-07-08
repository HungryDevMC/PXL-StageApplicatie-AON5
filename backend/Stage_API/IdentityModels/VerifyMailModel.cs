using System.ComponentModel.DataAnnotations;

namespace Stage_API.IdentityModels
{
    public class VerifyMailModel
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public string VerificationToken { get; set; }
    }
}