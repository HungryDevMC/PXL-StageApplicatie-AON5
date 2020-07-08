using Microsoft.AspNetCore.Identity;

namespace Stage_API
{
    public class TokenSettings : TokenOptions
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpirationTimeInMinutes { get; set; }
    }
}
