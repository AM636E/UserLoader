namespace UserLoader.WebApi.Authentication
{
    public class JwtConfiguration
    {
        public string Key { get; set; }
        public string Issuer { get; set; } = "http://localhost";
        public string Lifetime { get; set; } = "30";
    }
}