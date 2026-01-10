namespace Backend.Models.Auth
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = "ThisIsASecureSecretKeyForJWTAuthenticationThatIsAtLeast32BytesLong"; // 32+字符的安全密钥
        public string Issuer { get; set; } = "SchoolManagementSystem";
        public string Audience { get; set; } = "SchoolManagementSystem";
        public int ExpiresInMinutes { get; set; } = 60;
    }
}