namespace FlatFinder.WebAPI.Options
{
    public class TokenValidationOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string IssuerSigningKey { get; set; } // Don't do it!
    }
}