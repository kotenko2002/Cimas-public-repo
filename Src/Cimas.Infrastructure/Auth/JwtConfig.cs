namespace Cimas.Infrastructure.Auth
{
    public class JwtConfig
    {
        public const string Section = "Jwt";

        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
    }
}
