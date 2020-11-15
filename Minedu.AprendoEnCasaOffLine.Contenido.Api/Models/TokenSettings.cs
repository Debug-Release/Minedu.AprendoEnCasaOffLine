using Release.Helper;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api.Models
{
    public class TokenSettings
    {
        public string Secret { get; set; } = Functions.GetEnvironmentVariable("TOKEN_SECRET");
        public string User { get; set; } = Functions.GetEnvironmentVariable("TOKEN_USER");
        public string Password { get; set; } = Functions.GetEnvironmentVariable("TOKEN_PASSWORD");
        public string ClientId { get; set; } = Functions.GetEnvironmentVariable("TOKEN_CLIENT_ID");
        public string Expires { get; set; } = Functions.GetEnvironmentVariable("TOKEN_EXPIRES_DAYS");
    }
}
